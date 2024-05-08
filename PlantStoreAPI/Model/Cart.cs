using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    public class Cart
    {
        [Key, MaxLength(50), Required]
        public string? CustomerID { get; set; }

        [Key, MaxLength(50), Required]
        public string? ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }


        // Optional
        public Customer? Customer { get; set; } 

        public Product? Product { get; set; } 
    }
}
