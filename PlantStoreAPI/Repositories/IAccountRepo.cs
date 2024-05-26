using PlantStoreAPI.Model;
using PlantStoreAPI.Response;

namespace PlantStoreAPI.Repositories
{
    public interface IAccountRepo
    {
        Task<Customer> Register(Register user, string role);
        Task<LoginResponse> Login(Login user);
        Task Logout();
        Task AddFavouritePlants(string customerID, List<string> plants);
    }
}
