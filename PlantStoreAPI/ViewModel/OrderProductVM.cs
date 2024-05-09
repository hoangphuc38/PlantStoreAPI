using PlantStoreAPI.Model;

namespace PlantStoreAPI.ViewModel
{
    public class OrderProductVM
    {
        public string? OrderID { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public bool didFeedback { get; set; }
    }
}
