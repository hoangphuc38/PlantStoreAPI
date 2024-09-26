namespace PlantStoreAPI.ViewModel
{
    public class PaymentMethodVM
    {
        public int PaymentMethodId { get; set; }

        public string? PaymentTransactionNo { get; set; }

        public string? PaymentProvider { get; set; }

        public string? PaymentCartType { get; set; }

        public DateTime? PaymentDate { get; set; }

        // 0: unpaid, 1: paid, 2: failed
        public int? PaymentStatus { get; set; }

        public Boolean? IsDefault { get; set; } = false;

        public string? PaymentDescription { get; set; }

        public int PaymentTypeId { get; set; }
    }
}
