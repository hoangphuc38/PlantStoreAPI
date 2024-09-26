using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherRepo _repo;
        public VoucherController(IVoucherRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());    
        }

        [HttpGet("get-all/{customerID}")]
        public async Task<IActionResult> GetAllOfCustomer(string customerID)
        {
            return Ok(await _repo.GetAllOfCustomer(customerID));
        }

        [HttpGet("get-detail/{voucherID}")]
        public async Task<IActionResult> GetById(string voucherID)
        {
            return Ok(await _repo.GetById(voucherID));
        }

        [HttpPost("new-voucher")]
        public async Task<IActionResult> Add(VoucherVM voucher, int voucherTypeID)
        {
            return Ok(await _repo.Add(voucher, voucherTypeID));
        }

        [HttpPut("update-voucher/{voucherID}")]
        public async Task<IActionResult> Update(string voucherID, VoucherVM voucher, int voucherTypeID)
        {
            return Ok(await _repo.Update(voucherID, voucher, voucherTypeID));
        }

        [HttpDelete("delete-voucher/{voucherID}")]
        public async Task<IActionResult> Delete(string voucherID)
        {
            return Ok(await _repo.Delete(voucherID));
        }

        [HttpGet("search-by-name")]
        public async Task<IActionResult> SearchByName(string name)
        {
            return Ok(await _repo.SearchByName(name));
        }
    }
}
