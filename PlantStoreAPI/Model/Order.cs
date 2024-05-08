using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantStoreAPI.Model
{
    public class Order
    {
        [Key, Required]
        public string? OrderID { get; set; }

        [Required, MaxLength(50)]
        public string? CustomerID { get; set; }
        public DateTime TimeCreated { get; set; }
        public double TotalPrice { get; set; }
        [MaxLength(20)]
        public string? PayMethod { get; set; }
        public DateTime DeliveryDate { get; set; }
        [MaxLength(20)]
        public string? DeliveryMethod { get; set; }
        public double ShippingCost { get; set; }
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";
        [MaxLength(500)]
        public string? Note { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsPaid { get; set; }
        public string? VoucherID { get; set; }

        //optional
        public Customer? Customer { get; set; }
        [NotMapped]
        public Voucher? Voucher { get; set; }
    }
}
