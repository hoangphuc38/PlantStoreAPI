namespace PlantStoreAPI.ViewModel
{
    public class OrderVM
    {
        public string? CustomerID { get; set; }
        public double TotalPrice { get; set; }
        public string? DeliveryMethod { get; set; }       
        public double ShippingCost { get; set; }
        public string? Note { get; set; }
        public string? VoucherID { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public List<CartItemVM>? SelectedItems { get; set; }
    }
}
