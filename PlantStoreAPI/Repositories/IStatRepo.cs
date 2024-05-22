using PlantStoreAPI.Response;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IStatRepo
    {
        Task<StatisticResponse> GetRevenueToday();
        Task<StatisticResponse> GetRevenueThisMonth();
        Task<int> GetNumOfBillThisMonth();
        Task<double> GetTopDealToday();
        Task<List<StatVM>> RevenueByMonthsOfYear();
    }
}
