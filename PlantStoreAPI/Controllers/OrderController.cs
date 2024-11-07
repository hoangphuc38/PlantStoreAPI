using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _repo;
        public OrderController(IOrderRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-orders-admin")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int orderType = 0,
            [FromQuery] int month = 0,
            [FromQuery] bool today = false)
        {
            return Ok(await _repo.GetOrders(orderType, month, today));
        }

        [HttpGet("get-order-by-customer")]
        public async Task<IActionResult> GetByCustomer(string customerID, int orderType)
        {
            return Ok(await _repo.GetByCustomer(customerID, orderType));
        }

        [HttpPost("new-order")]
        public async Task<IActionResult> Add([FromBody] OrderVM order, int paymentType)
        {
            try
            {
                return Ok(new
                {
                    success = true,
                    data = await _repo.Add(order, paymentType)
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPut("update-status/{orderID}")]
        public async Task<IActionResult> UpdateStatus(string orderID)
        {
            return Ok(new
            {
                success = true,
                data = await (_repo.UpdateStatus(orderID))
            });
        }

        [HttpPut("update-payment-status/{orderID}")]
        public async Task<IActionResult> UpdatePaymentStatus(string orderID)
        {
            return Ok(new
            {
                success = true,
                data = await _repo.UpdatePaymentStatus(orderID)
            });
        }

        [HttpDelete("cancel-order/{orderID}/{isCancelByAdmin}")]
        public async Task<IActionResult> Cancel(string orderID, bool isCancelByAdmin)
        {
            try
            {
                return Ok(new
                {
                    success = true,
                    order = await _repo.Cancel(orderID, isCancelByAdmin)
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("search-by-id")]
        public async Task<IActionResult> SearchByID(string orderID)
        {
            return Ok(await _repo.SearchByID(orderID));
        }
    }
}
