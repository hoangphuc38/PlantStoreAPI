using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantStoreAPI.Model
{
    public class OrderDetail
    {
        [Key, Required]
        public string? OrderID { get; set; }

        [Key, MaxLength(50), Required]
        public string? ProductID { get; set; }
        public int Quantity { get; set; }
        public bool didFeedback { get; set; } = false;

        [NotMapped]
        //optional
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
