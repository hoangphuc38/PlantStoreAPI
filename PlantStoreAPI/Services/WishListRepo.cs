using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Services
{
    public class WishListRepo : IWishListRepo
    {
        public DataContext _context;
        public WishListRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<List<WishList>> GetByCustomer(string customerID)
        {
            var wishList = await _context.WishLists.Where(c => c.CustomerID == customerID).ToListAsync();

            if (wishList == null)
            {
                return new List<WishList>();
            }

            foreach (var wish in wishList)
            {
                var product = await _context.Products.FindAsync(wish.ProductID);
                var productImage = await _context.ProductImages
                                                .Where(c => c.ProductId == wish.ProductID)
                                                .ToListAsync();

                if (product == null)
                {
                    throw new KeyNotFoundException("Not found product");
                }

                product.Images = productImage;
                wish.Product = product;
            }

            return wishList;
        }
        public async Task<WishList> Add(WishListVM wishListVM)
        {
            var wishList = new WishList
            {
                CustomerID = wishListVM.CustomerID,
                ProductID = wishListVM.ProductID,
            };

            var customer = await _context.Customers.FindAsync(wishListVM.CustomerID);
            var product = await _context.Products.FindAsync(wishListVM.ProductID);

            if (customer == null || product == null)
            {
                throw new KeyNotFoundException();
            }

            wishList.Customer = customer;
            wishList.Product = product;

            _context.WishLists.Add(wishList);
            await _context.SaveChangesAsync();

            return wishList;
        }
        public async Task<WishList> Delete(string customerID, string productID)
        {
            var wishList = await _context.WishLists.FindAsync(customerID, productID);

            if (wishList == null)
            {
                throw new KeyNotFoundException();
            }

            _context.WishLists.Remove(wishList);
            await _context.SaveChangesAsync();

            return wishList;
        }
    }
}
