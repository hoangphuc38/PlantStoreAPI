using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface ICustomerRepo
    {
        Task<Customer> GetInfo(string customerID);
        Task<Customer> UpdateInfo(string customerID, CustomerVM customer);

    }
}
