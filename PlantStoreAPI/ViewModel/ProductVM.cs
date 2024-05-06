using PlantStoreAPI.Model;

namespace PlantStoreAPI.ViewModel
{
    public class ProductVM
    {
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? Height { get; set; }
        public string? Temperature { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? Images { get; set; } 
    }
}
