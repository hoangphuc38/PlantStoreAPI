namespace PlantStoreAPI.ViewModel
{
    public class MessageVM
    {
        public string CustomerID { get; set; } = "";
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsCustomerSend { get; set; }
    }
}
