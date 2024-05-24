using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Services
{
    public class OrderDetailRepo : IOrderDetailRepo
    {
        public DataContext _context;
        public OrderDetailRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<OrderDetailVM> GetByOrderID(string orderID)
        {
            var productInOrder = await _context.OrderDetails
                                               .Where(c => c.OrderID == orderID)
                                               .ToListAsync();
            OrderDetailVM vm = new OrderDetailVM
            {
                Products = new List<OrderProductVM>(),
            };

            var order = await _context.Orders.FindAsync(orderID);

            if (order != null)
            {
                vm.Order = order;

                if (order.VoucherID != null)
                {
                    var voucher = await _context.Vouchers.FindAsync(order.VoucherID);

                    if (voucher != null)
                    {
                        order.Voucher = voucher;
                    }
                }
            }

            foreach(var item in productInOrder)
            {
                var product = await _context.Products.FindAsync(item.ProductID);

                if (product != null)
                {
                    var orderProduct = new OrderProductVM
                    {
                        Product = product,
                        Quantity = item.Quantity,
                        didFeedback = item.didFeedback,
                        OrderID = orderID,
                    };
                    
                    orderProduct.Product.Images = await _context.ProductImages
                                                                .Where(c => c.ProductId == item.ProductID)
                                                                .ToListAsync();     
                    vm.Products.Add(orderProduct);
                }
            }

            return vm;
        }
        public async Task<List<OrderProductVM>> GetUnreviewedProducts(string customerID)
        {
            var orders = await _context.Orders
                                       .Where(c => c.CustomerID == customerID && c.Status == "Completed")
                                       .ToListAsync();

            List<OrderProductVM> products = new List<OrderProductVM>();

            foreach (var order in orders)
            {
                if (order.OrderID != null)
                {
                    var item = await GetByOrderID(order.OrderID);
                    var filteredItems = item.Products.FindAll(item => item.didFeedback == false);
                    products.AddRange(filteredItems);
                }               
            }

            return products;
        }
        public async Task<OrderDetail> Add(NewOrderDetailVM orderDetailVM)
        {
            var detail = new OrderDetail
            {
                OrderID = orderDetailVM.OrderID,
                ProductID = orderDetailVM.ProductID,
                Quantity = orderDetailVM.Quantity,
            };

            var order = await _context.Orders.FindAsync(orderDetailVM.OrderID);
            var product = await _context.Products.FindAsync(orderDetailVM.ProductID);

            if (order == null || product == null)
            {
                throw new KeyNotFoundException();
            }

            detail.Order = order;

            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync();
            return detail;
        }
        public async Task<OrderDetail> Delete(string customerID, string productID)
        {
            var order = await _context.OrderDetails.FindAsync(customerID, productID);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            _context.OrderDetails.Remove(order);

            await _context.SaveChangesAsync();

            return order;
        }
    }
}
