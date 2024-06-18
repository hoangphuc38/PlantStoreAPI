using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    public class Message
    {
        [Key, MaxLength(20), Required]
        public int MessageID { get; set; }

        [MaxLength(20), Required]
        public int RoomID { get; set; }

        [MaxLength(1000)]
        public string? Content { get; set; }
        public string? Image { get; set; }

        [Required]
        public DateTime SendTime { get; set; }

        [Required]
        public bool IsCustomerSend { get; set; }
        public ChatRoom Room { get; set; }
    }
}
