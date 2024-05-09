using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepo _repo;
        public CartController(ICartRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-cart-items/{customerID}")]
        public async Task<IActionResult> GetCartItems(string customerID)
        {
            return Ok(await _repo.GetCartItems(customerID));
        }

        [HttpGet("total-price/{customerID}")]
        public async Task<IActionResult> GetCartTotal(string customerID)
        {
            return Ok(await _repo.GetCartTotal(customerID));
        }

        [HttpGet("total-item/{customerID}")]
        public async Task<IActionResult> GetTotalItem(string customerID)
        {
            return Ok(await _repo.GetTotalItem(customerID));
        }

        [HttpGet("get-checkout-info/{customerID}")]
        public async Task<IActionResult> GetCheckoutInfo(string customerID)
        {
            return Ok(await _repo.GetCheckoutInfo(customerID));
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(CartVM cart)
        {
            if (cart.Quantity < 1)
            {
                return BadRequest();
            }

            return Ok(await _repo.AddToCart(cart));
        }

        [HttpPut("increase/{customerID}/{productID}")]
        public async Task<IActionResult> IncreaseQuantity(string customerID, string productID)
        {
            return Ok(await _repo.IncreaseQuantity(customerID, productID));
        }


        [HttpPut("decrease/{customerID}/{productID}")]
        public async Task<IActionResult> DecreaseQuantity(string customerID, string productID)
        {
            return Ok(await _repo.DecreaseQuantity(customerID, productID));
        }


        [HttpDelete("remove/{customerID}/{productID}")]
        public async Task<IActionResult> RemoveFromCart(string customerID, string productID)
        {
            return Ok(await _repo.RemoveFromCart(customerID, productID));
        }


        [HttpDelete("clear/{customerID}")]
        public async Task<IActionResult> ClearCart(string customerID)
        {
            await _repo.ClearCart(customerID);

            return Ok(
                new { success = true }
            );
        }
    }
}
