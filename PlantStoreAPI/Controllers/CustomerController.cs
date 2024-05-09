using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepo _repo;
        public CustomerController(ICustomerRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-info/{customerID}")]
        public async Task<IActionResult> GetInfo(string customerID)
        {
            return Ok(await _repo.GetInfo(customerID));
        }

        [HttpPut("update-info/{customerID}")]
        public async Task<IActionResult> UpdateInfo(string customerID, CustomerVM customer)
        {
            return Ok(await _repo.UpdateInfo(customerID, customer));
        }
    }
}
