using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantStoreAPI.Model
{
    [Table("PaymentType")]
    public class PaymentType
    {
        [Key]
        public int PaymentTypeId { get; set; }

        [Required, MaxLength(100)]
        public string PaymentTypeValue { get; set; }

        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
