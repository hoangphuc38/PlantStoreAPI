using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantStoreAPI.Model
{
    public class WishList
    {
        [Key, Required]
        public string? CustomerID { get; set; }

        [Key, Required]
        public string? ProductID { get; set; }

        [NotMapped]
        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
    }
}
