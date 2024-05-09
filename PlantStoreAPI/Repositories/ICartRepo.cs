using PlantStoreAPI.Model;
using PlantStoreAPI.Response;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface ICartRepo
    {
        Task<List<Cart>> GetCartItems(string customerID);
        Task<double?> GetCartTotal(string customerID);
        Task<int> GetTotalItem(string customerID);
        Task<CheckoutResponse> GetCheckoutInfo(string customerID);
        Task<Cart> AddToCart(CartVM cart);
        Task<Cart> RemoveFromCart(string customerID, string productID);
        Task ClearCart(string customerID);
        Task<Cart> IncreaseQuantity(string customerID, string productID);
        Task<Cart> DecreaseQuantity(string customerID, string productID);
    }
}
