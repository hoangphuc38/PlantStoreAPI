using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IWishListRepo
    {
        Task<List<WishList>> GetByCustomer(string customerID);  
        Task<WishList> Add(WishListVM wishList);
        Task<WishList> Delete(string customerID, string productID);
    }
}
