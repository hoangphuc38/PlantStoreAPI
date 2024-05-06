using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Services
{
    public class DeliveryInfoRepo : IDeliveryInfo
    {
        public DataContext _context;
        public DeliveryInfoRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<List<DeliveryInfo>> GetAll(string customerID)
        {
            var deliveryList = await _context.DeliveryInfos
                                    .Where(c => c.CustomerID == customerID)
                                    .ToListAsync();
            if (deliveryList == null)
            {
                deliveryList = new List<DeliveryInfo>();
            }

            return deliveryList;
        }
        public async Task<DeliveryInfo> Add(DeliveryInfoVM deliveryInfoVM)
        {
            var deliveryInfo = new DeliveryInfo
            {
                CustomerID = deliveryInfoVM.CustomerID,
                IsDefault = false,
                ReceiverName = deliveryInfoVM.Name,
                Address = deliveryInfoVM.Address,
                Phone = deliveryInfoVM.Phone,
            };

            _context.DeliveryInfos.Add(deliveryInfo);
            await _context.SaveChangesAsync();
            return deliveryInfo;
        }
    }
}
