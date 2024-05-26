using System.ComponentModel.DataAnnotations;

namespace PlantStoreAPI.Model
{
    public class FavouritePlant
    {
        [Key, MaxLength(50)]
        public string? CustomerID { get; set; }
        [Key]
        public string? PlantName { get; set; }
    }
}
