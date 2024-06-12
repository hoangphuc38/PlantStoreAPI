using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IDeliveryInfo
    {
        Task<List<DeliveryInfo>> GetAll(string customerID);
        Task<DeliveryInfo> GetDetail(int deliveryID);
        Task<DeliveryInfo> GetDefaultAddress(string customerID);
        Task<DeliveryInfo> Add(DeliveryInfoVM deliveryVM);
        Task<DeliveryInfo> Update(int deliveryID, DeliveryInfoVM deliveryVM);
        Task<DeliveryInfo> Delete(int deliveryID);
        Task<DeliveryInfo> SetDefault(string customerID, int deliveryID);
    }
}
