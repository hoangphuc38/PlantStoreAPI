using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? ProductId { get; set; }
        public string? ImageURL { get; set; }
    }
}
