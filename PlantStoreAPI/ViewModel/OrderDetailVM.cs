using PlantStoreAPI.Model;

namespace PlantStoreAPI.ViewModel
{
    public class OrderDetailVM
    {
        public Order? Order { get; set; }
        public List<OrderProductVM>? Products { get; set; }
    }
}
