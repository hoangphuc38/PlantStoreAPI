using PlantStoreAPI.Model;

namespace PlantStoreAPI.Response
{
    public class OrderResponse
    {
        public string? OrderID { get; set; }
        public double TotalPrice { get; set; }
        public string? Status { get; set; }
        public Product? FirstProduct { get; set; }
        public double TotalQuantity { get; set; }
        public DateTime TimeOrder { get; set; }
    }
}
