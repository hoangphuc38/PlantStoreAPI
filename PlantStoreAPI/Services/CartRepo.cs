using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.Response;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Services
{
    public class CartRepo : ICartRepo
    {
        public DataContext _context;
        public CartRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Cart>> GetCartItems(string customerID)
        {
            var carts = await _context.Carts
                .Where(c => c.CustomerID == customerID)
                .ToListAsync();

            foreach (var cart in carts)
            {
                cart.Product = await _context.Products.FindAsync(cart.ProductID);

                if (cart.Product != null)
                {
                    cart.Product.Images = await _context.ProductImages
                                                    .Where(c => c.ProductId == cart.ProductID)
                                                    .ToListAsync();
                }              
            }

            return carts;
        }
        public async Task<double?> GetCartTotal(string customerID)
        {
            return await _context.Carts
                .Where(c => c.CustomerID == customerID)
                .Select(c => c.Product.Price * c.Quantity)
                .SumAsync();
        }
        public async Task<int> GetTotalItem(string customerID)
        {
            return await _context.Carts
                .Where(c => c.CustomerID == customerID)
                .Select(c => c.Quantity)
                .SumAsync();
        }
        public async Task<CheckoutResponse> GetCheckoutInfo(string customerID)
        {
            var checkout = new CheckoutResponse
            {
                Items = await GetTotalItem(customerID),
                Total = (double) await GetCartTotal(customerID)
            };

            return checkout;
        }
        public async Task<Cart> AddToCart(CartVM cartVM)
        {
            var ifExistCart = await _context.Carts.FindAsync(cartVM.ProductID, cartVM.CustomerID);

            if (ifExistCart != null)
            {
                ifExistCart.Quantity += cartVM.Quantity;
                _context.Carts.Update(ifExistCart);
                await _context.SaveChangesAsync();
                return ifExistCart;
            }
            
            var cart = new Cart
            {
                CustomerID = cartVM.CustomerID,
                ProductID = cartVM.ProductID,
                Quantity = cartVM.Quantity,
            };

            var customer = await _context.Customers.FindAsync(cartVM.CustomerID);
            var product = await _context.Products.FindAsync(cartVM.ProductID);

            if (customer == null || product == null)
            {
                throw new KeyNotFoundException();
            }

            cart.Customer = customer;
            cart.Product = product;

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return cart;
        }
        public async Task<Cart> RemoveFromCart(string customerID, string productID)
        {
            var cart = await _context.Carts.FindAsync(customerID, productID);

            if (cart == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Carts.Remove(cart);

            _context.SaveChanges();

            return cart;
        }
        public async Task ClearCart(string customerID)
        {
            var carts = await _context.Carts
                .Where(c => c.CustomerID == customerID)
                .ToListAsync();

            _context.Carts.RemoveRange(carts);
            await _context.SaveChangesAsync();
        }
        public async Task<Cart> IncreaseQuantity(string customerID, string productID)
        {
            var cart = await _context.Carts.FindAsync(customerID, productID);

            if (cart == null)
            {
                throw new KeyNotFoundException();
            }

            var product = await _context.Products.FindAsync(cart.ProductID);
            
            if (product != null)
            {
                if (product.Quantity < cart.Quantity + 1)
                {
                    throw new Exception("Out of stock");
                }
            }

            cart.Quantity++;

            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();

            return cart;
        }
        public async Task<Cart> DecreaseQuantity(string customerID, string productID)
        {
            var cart = await _context.Carts.FindAsync(customerID, productID);

            if (cart == null)
            {
                throw new KeyNotFoundException();
            }

            if (cart.Quantity > 0)
            {
                cart.Quantity--;
            }

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return cart;
        }
    }
}
