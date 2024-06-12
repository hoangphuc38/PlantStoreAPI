using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepo _repo;
        public FeedbackController(IFeedbackRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-all-about-product/{productID}")]
        public async Task<IActionResult> GetAll(string productID)
        {
            return Ok(await _repo.GetAll(productID));
        }

        [HttpGet("get-all-reviews/{customerID}")]
        public async Task<IActionResult> GetAllByCustomer(string customerID)
        {
            return Ok(await _repo.GetAllByCustomer(customerID));
        }

        [HttpGet("avg-point/{productID}")]
        public async Task<IActionResult> GetAvgPoint(string productID)
        {
            return Ok(await _repo.GetAvgPoint(productID));
        }

        [HttpPost("new-feedback")]
        public async Task<IActionResult> Add(FeedbackVM feedback)
        {
            return Ok(await _repo.Add(feedback));
        }

        [HttpPut("update-feedback")]
        public async Task<IActionResult> Update(FeedbackVM feedback)
        {
            return Ok(await _repo.Update(feedback));
        }
        
    }
}
