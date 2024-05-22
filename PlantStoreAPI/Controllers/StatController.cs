using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatController : ControllerBase
    {
        private readonly IStatRepo _repo;
        public StatController(IStatRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-revenue-today")]
        public async Task<IActionResult> GetRevenueToday()
        {
            return Ok(await _repo.GetRevenueToday());
        }

        [HttpGet("get-revenue-thisMonth")]
        public async Task<IActionResult> GetRevenueThisMonth()
        {
            return Ok(await _repo.GetRevenueThisMonth());
        }

        [HttpGet("get-bills-thisMonth")]
        public async Task<IActionResult> GetNumOfBillThisMonth()
        {
            return Ok(await _repo.GetNumOfBillThisMonth());
        }

        [HttpGet("get-topDeal-today")]
        public async Task<IActionResult> GetTopDealToday()
        {
            return Ok(await _repo.GetTopDealToday());
        }

        [HttpGet("revenue-months-ofYear")]
        public async Task<IActionResult> RevenueByMonthsOfYear()
        {
            return Ok(await _repo.RevenueByMonthsOfYear());
        }
    }
}
