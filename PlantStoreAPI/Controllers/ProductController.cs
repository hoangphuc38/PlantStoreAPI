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
    }
}
