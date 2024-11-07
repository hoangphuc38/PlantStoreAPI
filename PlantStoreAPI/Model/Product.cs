using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PlantStoreAPI.Model
{
    public class Product
    {
        [Key, MaxLength(50), Required]
        public string? ProductID { get; set; }

        [Required, MaxLength(50)]
        public string? CategoryName { get; set; }

        [Required, MaxLength(50)]
        public string? ProductName { get; set; }
        
        public string? ProductNameVie { get; set; }

        public string? Height { get; set; }

        public string? Temperature { get; set; }

        public string? Size { get; set; }

        public int Quantity { get; set; }

        public double? Price { get; set; }

        public string? Description { get; set; }

        public double Sold { get; set; }

        public double? ReviewPoint { get; set; }

        public bool IsDeleted { get; set; } = false;

        [NotMapped]
        public List<ProductImage>? Images { get; set; }
    }
}
