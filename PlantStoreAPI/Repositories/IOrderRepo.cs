using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetOrders(
            int orderType = 0,
            int month = 0,
            bool today = false);
        Task<List<Order>> GetByCustomer(string customerID, int orderType);
        Task<Order> Add(OrderVM order);
        Task<Order> UpdateStatus(string orderID);
        Task<Order> UpdatePaymentStatus(string orderID);
        Task<Order> Cancel(string orderID, bool isCancelByAdmin);
    }
}
