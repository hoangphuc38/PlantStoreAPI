using PlantStoreAPI.Model;
using PlantStoreAPI.Response;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IOrderRepo
    {
        Task<List<OrderResponse>> GetOrders(
            int orderType = 0,
            int month = 0,
            bool today = false);
        Task<List<OrderResponse>> GetByCustomer(string customerID, int orderType);
        Task<Order> Add(OrderVM order);
        Task<Order> UpdateStatus(string orderID);
        Task<Order> UpdatePaymentStatus(string orderID);
        Task<Order> Cancel(string orderID, bool isCancelByAdmin);
        Task<List<OrderResponse>> SearchByID(string orderID);
    }
}
