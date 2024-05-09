using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IOrderDetailRepo
    {
        Task<OrderDetailVM> GetByOrderID(string orderID);
        Task<List<OrderProductVM>> GetUnreviewedProducts(string customerID);
        Task<OrderDetail> Add(NewOrderDetailVM orderDetail);
        Task<OrderDetail> Delete(string customerID, string productID);
    }
}
