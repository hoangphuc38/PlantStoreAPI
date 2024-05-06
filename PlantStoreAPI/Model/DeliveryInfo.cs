using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    public class DeliveryInfo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50), Required]
        public string? CustomerID { get; set; }
        public bool IsDefault { get; set; }
        public string? ReceiverName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

    }
}
