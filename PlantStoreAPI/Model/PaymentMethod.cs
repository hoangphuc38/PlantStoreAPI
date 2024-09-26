using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    [Table("PaymentMethod")]
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }

        public string? PaymentTransactionNo { get; set; }

        [MaxLength(255)]
        public string? PaymentProvider { get; set; }

        public string? PaymentCartType { get; set; }

        public DateTime? PaymentDate { get; set; }

        // 0: unpaid, 1: paid, 2: failed
        public int? PaymentStatus { get; set; }

        public Boolean? IsDefault { get; set; } = false;

        public string? PaymentDescription { get; set; }

        public int PaymentTypeId { get; set; }

        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType? PaymentType { get; set; }
    }
}
