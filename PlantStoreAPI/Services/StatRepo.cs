using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.Response;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Services
{
    public class StatRepo : IStatRepo
    {
        public DataContext _context;
        public StatRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<StatisticResponse> GetRevenueToday()
        {
            double revenue = await _context.Orders
                                            .Where(c => c.TimeCreated.Date == DateTime.Now.Date)
                                            .Select(c => c.TotalPrice)
                                            .SumAsync();
            var stat = new StatisticResponse
            {
                Revenue = revenue,
            };

            return stat;
        }
        public async Task<StatisticResponse> GetRevenueThisMonth()
        {
            double revenue = await _context.Orders
                                            .Where(c => c.TimeCreated.Month == DateTime.Now.Month 
                                                     && c.TimeCreated.Year == DateTime.Now.Year)
                                            .Select(c => c.TotalPrice)
                                            .SumAsync();
            var stat = new StatisticResponse
            {
                Revenue = revenue,
            };

            return stat;
        }
        public async Task<int> GetNumOfBillThisMonth()
        {
            int numofBill = await _context.Orders
                                          .Where(c => c.TimeCreated.Month == DateTime.Now.Month
                                                   && c.TimeCreated.Year == DateTime.Now.Year)
                                          .CountAsync();
            return numofBill;
        }
        public async Task<double> GetTopDealToday()
        {
            var topDealValue = await _context.Orders.Where(c => c.TimeCreated.Date == DateTime.Now.Date)
                                            .MaxAsync(c => c.TotalPrice);
            return topDealValue;
        }
        public async Task<List<StatVM>> RevenueByMonthsOfYear()
        {
            var revenueList = new List<StatVM>();

            for (int i = 0; i < DateTime.Now.Month; i++)
            {
                var revenue = new StatVM
                {
                    Month = i + 1,
                    Revenue = await _context.Orders
                                            .Where(o => o.TimeCreated.Month == i + 1 &&
                                                        o.TimeCreated.Year == DateTime.Now.Year)
                                            .Select(o => o.TotalPrice)
                                            .SumAsync()
                };
                revenueList.Add(revenue);
            }

            return revenueList;
        }
    }
}
