using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    public class CustomerType
    {
        [Key, Required]
        public int CustomerTypeId { get; set; }

        [MaxLength(255)]
        public string? CustomerTypeName { get; set; } 

        public double MaxPaid { get; set; }
    }
}
