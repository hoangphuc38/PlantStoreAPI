using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    public class VoucherApplied
    {
        [Key, MaxLength(50), Required]
        public string? VoucherID { get; set; }
        [Key, MaxLength(50), Required]
        public string? CustomerID { get; set; }
    }
}
