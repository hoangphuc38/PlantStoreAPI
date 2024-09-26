using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IVoucherRepo
    {
        Task<List<Voucher>> GetAll();
        Task<List<Voucher>> GetAllOfCustomer(string customerID);
        Task<Voucher> GetById(string voucherID);
        Task<Voucher> Add(VoucherVM voucher, int voucherTypeID);
        Task<Voucher> Delete(string voucherId);
        Task<Voucher> Update(string voucherId, VoucherVM voucher, int voucherTypeID);
        Task<List<Voucher>> SearchByName(string name);
    }
}
