using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepo _repo;
        public OrderDetailController(IOrderDetailRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-detail/{orderID}")]
        public async Task<IActionResult> GetByOrderID(string orderID)
        {
            return Ok(await _repo.GetByOrderID(orderID));
        }

        [HttpGet("get-unreviewed-products/{customerID}")]
        public async Task<IActionResult> GetUnreviewedProducts(string customerID)
        {
            return Ok(await _repo.GetUnreviewedProducts(customerID));
        }

        [HttpPost("add-new")]
        public async Task<IActionResult> Add([FromBody] NewOrderDetailVM orderDetail)
        {
            return Ok(await _repo.Add(orderDetail));    
        }

        [HttpDelete("remove-item-from-order/{customerID}/{productID}")]
        public async Task<IActionResult> Delete(string customerID, string productID)
        {
            return Ok(await _repo.Delete(customerID, productID));
        }

    }
}
