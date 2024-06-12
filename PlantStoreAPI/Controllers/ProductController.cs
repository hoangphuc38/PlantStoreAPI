using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _repo;
        public ProductController(IProductRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [HttpGet("get-by-category/{categoryName}")]
        public async Task<IActionResult> GetByCategory(string categoryName)
        {
            return Ok(await _repo.GetByCategory(categoryName));
        }

        [HttpGet("get-best-seller")]
        public async Task<IActionResult> GetBestSeller()
        {
            return Ok(await _repo.GetBestSeller());
        }

        [HttpGet("detail/{productID}")]
        public async Task<IActionResult> GetDetail(string productID)
        {
            return Ok(await _repo.GetDetail(productID));
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> Add([FromForm] ProductVM product)
        {
            return Ok(await _repo.Add(product));
        }

        [HttpPut("update-product/{productId}")]
        public async Task<IActionResult> Update(string productId, [FromForm] ProductVM product)
        {
            return Ok(await _repo.Update(productId, product));
        }

        [HttpDelete("delete-product/{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            return Ok(await _repo.Delete(productId)); 
        }

        [HttpGet("search-by-name")]
        public async Task<IActionResult> SearchByName(string name)
        {
            return Ok(await _repo.SearchByName(name));
        }

        [HttpGet("recommend/{customerID}")]
        public async Task<IActionResult> RecommendProducts(string customerID)
        {
            return Ok(await _repo.RecommendProducts(customerID));
        }

        [HttpPost("predict-by-image")]
        public async Task<IActionResult> SearchByImage([FromForm] SearchImageVM image)
        {
            return Ok(await _repo.SearchByImage(image));
        }
    }
}
