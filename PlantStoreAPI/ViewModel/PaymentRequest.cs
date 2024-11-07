namespace PlantStoreAPI.ViewModel
{
    public class PaymentRequest
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public double OrderTotal { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
