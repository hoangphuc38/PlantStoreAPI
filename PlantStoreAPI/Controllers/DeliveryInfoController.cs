using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryInfoController : ControllerBase
    {
        private readonly IDeliveryInfo _repo;
        public DeliveryInfoController(IDeliveryInfo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(string customerID)
        {
            return Ok(await _repo.GetAll(customerID));
        }

        [HttpPost("new-address")]
        public async Task<IActionResult> Add(DeliveryInfoVM deliveryInfo)
        {
            return Ok(await _repo.Add(deliveryInfo));
        }
    }
}
