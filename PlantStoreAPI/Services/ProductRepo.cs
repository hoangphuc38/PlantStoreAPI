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
                product.ReviewPoint = 0; //await GetAvgPoint(product.ProductID);
                product.Sold = 0; //await Sold(product.ProductID);    
                product.Images = await _context.ProductImages
                                    .Where(c => c.ProductId == product.ProductID)
                                    .ToListAsync();
            }

            return products;
        }
        public async Task<Product> Add(ProductVM productVM)
        {
            string autoID = await AutoID();

            var product = new Product
            {
                ProductID = autoID,
                ProductName = productVM.ProductName,
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

            //productVM.Images = new List<IFormFile>();       

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

            _context.Products.Add(product);
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
        //public async Task<double> GetAvgPoint(string productID)
        //{
        //    return 0; //Bổ sung sau khi tạo bảng Feedback
        //}

        //public async Task<int> Sold(string productID)
        //{
        //    return 0; //Bổ sung sau khi tạo bảng Order
        //}
    }
}
