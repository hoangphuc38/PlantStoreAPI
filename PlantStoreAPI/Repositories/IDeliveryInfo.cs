using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IDeliveryInfo
    {
        Task<List<DeliveryInfo>> GetAll(string customerID);
        Task<DeliveryInfo> Add(DeliveryInfoVM deliveryVM);
    }
}
