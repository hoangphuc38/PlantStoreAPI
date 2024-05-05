using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IProductRepo
    {
        Task<List<Product>> GetAll();
        Task<Product> Add(ProductVM productVM);
        
    }
}
