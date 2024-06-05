using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.Response;
using PlantStoreAPI.StaticServices;
using PlantStoreAPI.ViewModel;
using PlantStoreAPI.Resources;
using System.Text.RegularExpressions;

namespace PlantStoreAPI.Services
{
    public class OrderRepo : IOrderRepo
    {
        public DataContext _context;
        public OrderRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<List<OrderResponse>> GetOrders(
            int orderType = 0,
            int month = 0,
            bool today = false)
        {
            List<OrderResponse> orderResponses = new List<OrderResponse>();

            var status = orderType == 0 ? "All" :
                         orderType == 1 ? "Pending" :
                         orderType == 2 ? "Packaging" :
                         orderType == 3 ? "Delivering" :
                         "Completed";

            var orders = status == "All"
                ? await _context.Orders.ToListAsync()
                : await _context.Orders.Where(c => c.Status == status).ToListAsync();

            if (month > 0 && month <= 12)
            {
                orders = orders.Where(o => o.TimeCreated.Month == month).ToList();
            }

            if (today)
            {
                orders = orders.Where(o => o.TimeCreated.Date == DateTime.Now.Date).ToList();
            }

            foreach (var order in orders)
            {
                OrderResponse orderResponse = new OrderResponse
                {
                    OrderID = order.OrderID,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status,
                    TimeOrder = order.TimeCreated,
                };

                var orderDetail = await _context.OrderDetails
                                                .Where(c => c.OrderID == order.OrderID)
                                                .FirstOrDefaultAsync();

                if (orderDetail != null)
                {
                    orderResponse.FirstProduct = await _context.Products
                                                           .Where(c => c.ProductID == orderDetail.ProductID)
                                                           .FirstAsync();

                    orderResponse.FirstProduct.Images = await _context.ProductImages
                                                           .Where(c => c.ProductId == orderDetail.ProductID)
                                                           .ToListAsync();
                }

                orderResponse.TotalQuantity = await _context.OrderDetails
                                                            .Where(c => c.OrderID == order.OrderID)
                                                            .Select(c => c.Quantity)
                                                            .SumAsync();
                orderResponses.Add(orderResponse);
            }

            return orderResponses;
        }
        public async Task<List<OrderResponse>> GetByCustomer(string customerID, int orderType)
        {
            List<OrderResponse> orderResponses = new List<OrderResponse>();

            var status = orderType == 0 ? "All" :
                         orderType == 1 ? "Pending" :
                         orderType == 2 ? "Packaging" :
                         orderType == 3 ? "Delivering" :
                         orderType == 4 ? "Completed" :
                         "Canceled";

            List<Order> orders = new List<Order>();

            if (status == "All")
            {
                orders = await _context.Orders
                                           .OrderByDescending(x => x.TimeCreated)
                                           .Where(o => o.CustomerID == customerID)
                                           .ToListAsync();               
            }
            else
            {
                orders = await _context.Orders
                                           .Where(o => o.CustomerID == customerID &&
                                                       o.Status == status)
                                           .OrderByDescending(x => x.TimeCreated)
                                           .ToListAsync();
            }

            foreach (var order in orders)
            {
                OrderResponse orderResponse = new OrderResponse
                {
                    OrderID = order.OrderID,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status,
                    TimeOrder = order.TimeCreated,
                };

                var orderDetail = await _context.OrderDetails
                                                .Where(c => c.OrderID == order.OrderID)
                                                .FirstOrDefaultAsync();

                if (orderDetail != null)
                {
                    orderResponse.FirstProduct = await _context.Products
                                                           .Where(c => c.ProductID == orderDetail.ProductID)
                                                           .FirstAsync();

                    orderResponse.FirstProduct.Images = await _context.ProductImages
                                                           .Where(c => c.ProductId == orderDetail.ProductID)
                                                           .ToListAsync();
                }

                orderResponse.TotalQuantity = await _context.OrderDetails
                                                            .Where(c => c.OrderID == order.OrderID)
                                                            .Select(c => c.Quantity)
                                                            .SumAsync();
                orderResponses.Add(orderResponse);
            }

            return orderResponses;
        }
        public async Task<Order> Add(OrderVM orderVM)
        {
            var order = new Order
            {
                OrderID = await AutoID(),
                CustomerID = orderVM.CustomerID,
                TimeCreated = DateTime.Now,
                TotalPrice = orderVM.TotalPrice,
                PayMethod = orderVM.PayMethod,
                DeliveryMethod = orderVM.DeliveryMethod,
                ShippingCost = orderVM.ShippingCost,
                Status = "Pending",
                Note = orderVM.Note,
                Name = orderVM.Name,
                Phone = orderVM.Phone,
                Address = orderVM.Address,
                VoucherID = orderVM.VoucherID,
                IsPaid = false,
            };

            switch (orderVM.DeliveryMethod?.ToUpper())
            {
                case "NORMAL":
                    order.DeliveryDate = DateTime.Now.AddDays(4);
                    break;
                case "EXPRESS":
                    order.DeliveryDate = DateTime.Now.AddDays(2);
                    break;
            }

            var customer = await _context.Customers.FindAsync(orderVM.CustomerID);

            if (customer == null)
            {
                throw new KeyNotFoundException();
            }

            order.Customer = customer;

            if (orderVM.VoucherID != "")
            {
                var voucher = await _context.Vouchers.FindAsync(orderVM.VoucherID);
                order.Voucher = voucher;

                var voucherDetail = await _context.VoucherApplied.FindAsync(orderVM.VoucherID, order.CustomerID);

                if (voucherDetail != null)
                {
                    _context.VoucherApplied.Remove(voucherDetail);
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var cartItems = orderVM.SelectedItems;

            if (cartItems != null)
            {
                foreach (var cartItem in cartItems)
                {
                    var product = await _context.Products.FindAsync(cartItem.ProductID);
                    
                    if (product != null)
                    {
                        _context.OrderDetails.Add(new OrderDetail
                        {
                            OrderID = order.OrderID,
                            ProductID = cartItem.ProductID,
                            Quantity = cartItem.Quantity,
                        });

                        product.Quantity -= cartItem.Quantity;
                        product.Sold += cartItem.Quantity;

                        if (product.Quantity < 0)
                        {
                            product.Quantity += cartItem.Quantity;
                            throw new Exception($"Not enough quantity for {product.ProductName}");
                        }

                        _context.Products.Update(product);
                    }

                    var record = await _context.Carts.FindAsync(orderVM.CustomerID, cartItem.ProductID);

                    if (record != null)
                    {
                        _context.Carts.Remove(record);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return order;
        }
        public async Task<Order> UpdateStatus(string orderID)
        {
            var order = await _context.Orders.FindAsync(orderID);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            if (order.Status == "Completed")
            {
                return order;
            }

            var customer = await _context.Customers.FindAsync(order.CustomerID);
            order.Customer = customer;

            var voucher = await _context.Vouchers.FindAsync(order.VoucherID);
            order.Voucher = voucher;

            var status = new List<string>
            {
                "Pending",
                "Packaging",
                "Delivering",
                "Completed"
            };

            int index = status.IndexOf(order.Status);

            order.Status = status[index + 1];

            var user = await _context.Users.FindAsync(order.CustomerID);

            if (user != null)
            {
                string content = "", subject = "";
                switch (order.Status)
                {
                    case "Packaging":
                        content = await ConfirmEmailContent(order);
                        subject = "[PK PLANT STORE] Đơn hàng của bạn đã được xác nhận!";
                        break;
                    case "Delivering":
                        content = UpdateStatusContent(order);
                        subject = $"[PK PLANT STORE] Cập nhật thông tin giao hàng cho đơn hàng #{order.OrderID}";
                        break;
                    case "Completed":
                        content = DeliveredContent(order);
                        subject = $"[PK PLANT STORE] Đơn hàng #{order.OrderID} đã hoàn thành!";
                        order.IsPaid = true;
                        break;
                }

                if (content != "")
                {
                    Gmail.SendEmail(subject, content, new List<string> { user.Email });
                }
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }
        public async Task<Order> UpdatePaymentStatus(string orderID)
        {
            var order = await _context.Orders.FindAsync(orderID);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            order.IsPaid = true;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }
        public async Task<Order> Cancel(string orderID, bool isCancelByAdmin = false)
        {
            var order = await _context.Orders.FindAsync(orderID);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            if (order.Status != "Pending" && order.Status != "Packaging" && !isCancelByAdmin)
            {
                throw new Exception("Only cancel order before delivering");
            }

            if (order.Status == "Completed")
            {
                throw new Exception("Cannot cancel completed order");
            }

            if (order.Status != "Delivering" && isCancelByAdmin)
            {
                throw new Exception("Only Admin can cancel delivering order");
            }

            var orderDetails = await _context.OrderDetails
                .Where(o => o.OrderID == orderID)
                .ToListAsync();

            foreach (var detail in orderDetails)
            {
                var product = await _context.Products.FindAsync(detail.ProductID);

                if (product == null)
                {
                    continue;
                }

                product.Quantity += detail.Quantity;

                _context.Products.Update(product);
            }

            if (!isCancelByAdmin)
            {
                var voucher = await _context.Vouchers.FindAsync(order.VoucherID);

                order.Status = "Canceled";
                _context.Orders.Update(order);

                if (voucher != null)
                {
                    _context.VoucherApplied.Add(new VoucherApplied
                    {
                        VoucherID = order.VoucherID,
                        CustomerID = order.CustomerID
                    });
                }
            }

            await _context.SaveChangesAsync();

            return order;
        }
        public async Task<List<OrderResponse>> SearchByID(string orderID)
        {
            List<OrderResponse> orderResponses = new List<OrderResponse>();

            var orders = await _context.Orders.Where(c => c.OrderID.Contains(orderID)).ToListAsync();

            foreach (var order in orders)
            {
                OrderResponse orderResponse = new OrderResponse
                {
                    OrderID = order.OrderID,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status,
                    TimeOrder = order.TimeCreated,
                };

                var orderDetail = await _context.OrderDetails
                                                .Where(c => c.OrderID == order.OrderID)
                                                .FirstOrDefaultAsync();

                if (orderDetail != null)
                {
                    orderResponse.FirstProduct = await _context.Products
                                                           .Where(c => c.ProductID == orderDetail.ProductID)
                                                           .FirstAsync();

                    orderResponse.FirstProduct.Images = await _context.ProductImages
                                                           .Where(c => c.ProductId == orderDetail.ProductID)
                                                           .ToListAsync();
                }

                orderResponse.TotalQuantity = await _context.OrderDetails
                                                            .Where(c => c.OrderID == order.OrderID)
                                                            .Select(c => c.Quantity)
                                                            .SumAsync();
                orderResponses.Add(orderResponse);
            }

            return orderResponses;
        }
        private async Task<string> ConfirmEmailContent(Order order)
        {
            var confirmEmail = PKRes.ConfirmEmail;

            var details = await _context.OrderDetails
                .Where(o => o.OrderID == order.OrderID)
                .ToListAsync();

            double? subTotal = 0;

            foreach (var detail in details)
            {
                var detailContent = PKRes.ProductDetail;

                detail.Product = await _context.Products.FindAsync(detail.ProductID);

                detailContent = detailContent.Replace("{{ProductName}}", detail.Product?.ProductName);
                detailContent = detailContent.Replace("{{Quantity}}", detail.Quantity.ToString());
                detailContent = detailContent.Replace("{{ProductPrice}}", detail.Product?.Price.ToString());

                confirmEmail.Insert(confirmEmail.IndexOf("<!-- detail -->"), detailContent + "\n");

                subTotal += detail.Product?.Price;
            }

            if (order.OrderID != null)
            {
                confirmEmail = confirmEmail.Replace("{{CustomerName}}", order.Customer?.Name);
                confirmEmail = confirmEmail.Replace("{{OrderID}}", order.OrderID.ToString());
                confirmEmail = confirmEmail.Replace("{{SubTotal}}", subTotal.ToString());
                confirmEmail = confirmEmail.Replace("{{Shipping}}", order.ShippingCost.ToString());
            }

            if (order.Voucher != null)
            {
                confirmEmail = confirmEmail.Replace("{{Discount}}", order.Voucher.Value.ToString());
            }
            else
            {
                confirmEmail = confirmEmail.Replace("{{Discount}}", "0");
            }
            confirmEmail = confirmEmail.Replace("{{Total}}", order.TotalPrice.ToString());
            confirmEmail = confirmEmail.Replace("{{TimeCreated}}", order.TimeCreated.ToShortDateString());
            confirmEmail = confirmEmail.Replace("{{Name}}", order.Name);
            confirmEmail = confirmEmail.Replace("{{Phone}}", order.Phone);
            confirmEmail = confirmEmail.Replace("{{Address}}", order.Address);

            return confirmEmail;
        }

        private string UpdateStatusContent(Order order)
        {
            var content = PKRes.UpdateStatus;

            if (order.OrderID != null)
            {
                content = content.Replace("{{CustomerName}}", order.Customer?.Name);
                content = content.Replace("{{OrderID}}", order.OrderID.ToString());
                content = content.Replace("{{OrderStatus}}", "Đang vận chuyển");
                content = content.Replace("{{Total}}", order.TotalPrice.ToString());
            }

            return content;
        }

        private string DeliveredContent(Order order)
        {          
            var content = PKRes.Delivered;
            if (order.OrderID != null)
            {
                content = content.Replace("{{CustomerName}}", order.Customer?.Name);
                content = content.Replace("{{OrderID}}", order.OrderID.ToString());
                content = content.Replace("{{OrderStatus}}", "Đã giao hàng");
                content = content.Replace("{{Total}}", order.TotalPrice.ToString());
            }

            return content;
        }
        private async Task<string> AutoID()
        {
            var ID = "HD0001";

            var maxID = await _context.Orders
                .OrderByDescending(v => v.OrderID)
                .Select(v => v.OrderID)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxID))
            {
                return ID;
            }

            var numeric = Regex.Match(maxID, @"\d+").Value;

            if (string.IsNullOrEmpty(numeric))
            {
                return ID;
            }

            ID = "HD";

            numeric = (int.Parse(numeric) + 1).ToString();

            while (ID.Length + numeric.Length < 6)
            {
                ID += '0';
            }

            return ID + numeric;
        }
    }
}
