using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IProductRepo
    {
        Task<List<Product>> GetAll();
        Task<List<Product>> GetByCategory(string categoryName);
        Task<Product> Add(ProductVM productVM);
        Task<Product> Update(string productID, ProductVM productVM);
        Task<Product> Delete(string productID);
        
    }
}
