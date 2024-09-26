using PlantStoreAPI.Model;

namespace PlantStoreAPI.ViewModel
{
    public class CustomerVM
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public bool Male { get; set; }
        public string? Address { get; set; }
        public DateTime DateBirth { get; set; }
    }
}
