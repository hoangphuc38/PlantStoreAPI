using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListRepo _repo;
        public WishListController(IWishListRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-all/{customerID}")]
        public async Task<IActionResult> GetByCustomer(string customerID)
        {
            return Ok(await _repo.GetByCustomer(customerID));
        }

        [HttpPost("add-wishlist")]
        public async Task<IActionResult> Add(WishListVM wishList)
        {
            return Ok(await _repo.Add(wishList));
        }

        [HttpDelete("remove-from-wishlist")]
        public async Task<IActionResult> Delete(string customerID, string productID)
        {
            return Ok(await _repo.Delete(customerID, productID));
        }
    }
}
