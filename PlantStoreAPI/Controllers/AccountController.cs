using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepo _repo;
        public AccountController(IAccountRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register register, string role)
        {
            try
            {
                return Ok(new
                {
                    message = "Success",
                    data = await _repo.Register(register, role)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login user)
        {
            try
            {
                var result = await _repo.Login(user);

                return Ok(new
                {
                    message = "Success",
                    data = result
                });
            }
            catch (ArgumentNullException)
            {
                return NotFound("User not found!");
            }
            catch (KeyNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _repo.Logout();
            return Ok(new
            {
                message = "Success"
            });
        }

        [HttpPost("set-favourite-plants/{customerID}")]
        public async Task<IActionResult> SetFavouritePlants(string customerID, [FromBody] List<string> plants)
        {
            await _repo.AddFavouritePlants(customerID, plants);
            return Ok();
        }
    }
}
