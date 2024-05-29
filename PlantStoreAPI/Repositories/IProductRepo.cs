using PlantStoreAPI.Model;
using PlantStoreAPI.Response;
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
        Task<List<Product>> SearchByName(string name);
        Task<List<RecommendationVM>> RecommendProducts(string customerID);
        Task<SearchImageResponse> SearchByImage(SearchImageVM image);
    }
}
