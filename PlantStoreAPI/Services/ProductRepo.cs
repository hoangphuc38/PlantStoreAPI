using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.StaticServices;
using PlantStoreAPI.ViewModel;
using System.Text.RegularExpressions;

namespace PlantStoreAPI.Services
{
    public class ProductRepo : IProductRepo
    {
        public DataContext _context;
        public ProductRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAll()
        {
            var products = await _context.Products.ToListAsync();

            foreach (var product in products)
            {
                if (product.ProductID != null)
                {
                    product.Images = await _context.ProductImages
                                        .Where(c => c.ProductId == product.ProductID)
                                        .ToListAsync();
                }              
            }

            return products;
        }
        public async Task<List<Product>> GetByCategory(string categoryName)
        {
            var productList = await _context.Products.Where(c => c.CategoryName == categoryName).ToListAsync();

            if (productList == null)
            {
                return new List<Product>();
            }
            
            foreach (var product in productList)
            {
                product.Images = new List<ProductImage>();
                var productImage = await _context.ProductImages
                                        .Where(c => c.ProductId == product.ProductID)
                                        .ToListAsync();

                product.Images.AddRange(productImage);               
            }

            return productList;
        }
        public async Task<Product> Add(ProductVM productVM)
        {
            string autoID = await AutoID();

            var product = new Product
            {
                ProductID = autoID,
                ProductName = productVM.ProductName,
                CategoryName = productVM.CategoryName,
                Height = productVM.Height,
                Temperature = productVM.Temperature,
                Size = productVM.Size,
                Quantity = productVM.Quantity,
                Price = productVM.Price,
                Description = productVM.Description,
                Sold = 0,
                ReviewPoint = 0,
                Images = new List<ProductImage>()
            };
            
            if (productVM.Images == null)
            {

            }
            else
            {
                foreach (IFormFile file in productVM.Images)
                {
                    ProductImage image = new ProductImage
                    {
                        ProductId = autoID,
                        ImageURL = await UploadImage.Instance.UploadAsync(file)
                    };
                    _context.ProductImages.Add(image);

                    product.Images.Add(image);
                }
            }          

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product> Update(string productID, ProductVM productVM)
        {
            var product = await _context.Products.FindAsync(productID);

            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            product.ProductName = productVM.ProductName;
            product.CategoryName = productVM.CategoryName;
            product.Height = productVM.Height;
            product.Temperature = productVM.Temperature;
            product.Size = productVM.Size;
            product.Quantity = productVM.Quantity;
            product.Price = productVM.Price;
            product.Description = productVM.Description;
            product.Images = new List<ProductImage>();

            var productImageList = await _context.ProductImages
                                            .Where(c => c.ProductId == productID)
                                            .ToListAsync();           
            if (productVM.Images == null)
            {
                product.Images = productImageList;
            }
            else
            {
                _context.ProductImages.RemoveRange(productImageList);
                await _context.SaveChangesAsync();

                foreach (IFormFile file in productVM.Images)
                {
                    ProductImage image = new ProductImage
                    {
                        ProductId = productID,
                        ImageURL = await UploadImage.Instance.UploadAsync(file)
                    };
                    _context.ProductImages.Add(image);

                    product.Images.Add(image);
                }
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product> Delete(string productID)
        {
            var product = await _context.Products.FindAsync(productID);
            var productImageList = await _context.ProductImages
                                        .Where(c => c.ProductId == productID)
                                        .ToListAsync();

            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            _context.RemoveRange(productImageList);
            _context.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        private async Task<string> AutoID()
        {
            var ID = "PD0001";

            var maxID = await _context.Products
                .OrderByDescending(v => v.ProductID)
                .Select(v => v.ProductID)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxID))
            {
                return ID;
            }

            ID = "PD";

            var numeric = Regex.Match(maxID, @"\d+").Value;

            numeric = (int.Parse(numeric) + 1).ToString();

            while (ID.Length + numeric.Length < 6)
            {
                ID += '0';
            }

            return ID + numeric;
        }
        public async Task<double> GetAvgPoint(string productID)
        {
            return await _context.Feedbacks.Where(f => f.ProductID == productID)
                .Select(f => f.Point)
                .AverageAsync();
        }

        public async Task<double> Sold(string productID)
        {
            return await _context.Products.Where(c => c.ProductID == productID)
                                          .Select(c => c.Sold)
                                          .FirstOrDefaultAsync();
        }
    }
}
