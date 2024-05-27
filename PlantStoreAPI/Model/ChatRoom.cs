using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    public class ChatRoom
    {
        [Key, MaxLength(20), Required]
        public int RoomID { get; set; }
        [MaxLength(50), Required]
        public string? CustomerID { get; set; }
        public Customer Customer { get; set; } = new Customer();
    }
}
