using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    public class VoucherType
    {
        [Key, Required]
        public int VoucherTypeId { get; set; }

        [MaxLength(255)]
        public string? VoucherTypeName { get; set; }
    }
}
