using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IVoucherRepo
    {
        Task<List<Voucher>> GetAll();
        Task<Voucher> GetById(string voucherID);
        Task<Voucher> Add(VoucherVM voucher);
        Task<Voucher> Delete(string voucherId);
        Task<Voucher> Update(string voucherId, VoucherVM voucher);
    }
}
