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
        public async Task<DeliveryInfo> GetDetail(int deliveryID)
        {
            var delivery = await _context.DeliveryInfos.FindAsync(deliveryID);

            if (delivery == null)
            {
                throw new Exception("Not found");
            }

            return delivery;
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
        public async Task<DeliveryInfo> Delete(int deliveryID)
        {
            var deliveryInfo = await _context.DeliveryInfos.FindAsync(deliveryID);

            if (deliveryInfo == null)
            {
                throw new KeyNotFoundException();
            }

            _context.DeliveryInfos.Remove(deliveryInfo);
            await _context.SaveChangesAsync();
            return deliveryInfo;
        }
        public async Task<DeliveryInfo> Update(int deliveryID, DeliveryInfoVM deliveryInfoVM)
        {
            var deliveryInfo = await _context.DeliveryInfos.FindAsync(deliveryID);

            if (deliveryInfo == null)
            {
                throw new KeyNotFoundException();
            }

            deliveryInfo.CustomerID = deliveryInfoVM.CustomerID;
            deliveryInfo.IsDefault = deliveryInfo.IsDefault;
            deliveryInfo.ReceiverName = deliveryInfoVM.Name;
            deliveryInfo.Address = deliveryInfoVM.Address;
            deliveryInfo.Phone = deliveryInfoVM.Phone;

            _context.DeliveryInfos.Update(deliveryInfo);
            await _context.SaveChangesAsync();
            return deliveryInfo;
        }
        public async Task<DeliveryInfo> SetDefault(string customerID, int deliveryID)
        {
            var deliveryList = await _context.DeliveryInfos.Where(c => c.CustomerID == customerID).ToListAsync();

            if (deliveryList == null)
            {
                throw new Exception("Not found any delivery infos");
            }

            foreach (var deliveryInfo in deliveryList)
            {
                deliveryInfo.IsDefault = false;
            }

            _context.DeliveryInfos.UpdateRange(deliveryList);
            await _context.SaveChangesAsync();

            var deliveryInfoUpdate = await _context.DeliveryInfos.FindAsync(deliveryID);

            if (deliveryInfoUpdate == null)
            {
                throw new Exception("Not found this delivery info");
            }

            deliveryInfoUpdate.IsDefault = true;
            _context.DeliveryInfos.Update(deliveryInfoUpdate);
            await _context.SaveChangesAsync();

            return deliveryInfoUpdate;                                              
        }
    }
}
