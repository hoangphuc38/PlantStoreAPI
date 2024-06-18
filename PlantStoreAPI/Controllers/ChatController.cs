using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlantStoreAPI.Hubs;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepo _repo;
        private readonly IHubContext<ChatHub> _hub;
        public ChatController(IChatRepo repo, IHubContext<ChatHub> hub)
        {
            _repo = repo;
            _hub = hub;
        }

        [HttpGet("get-all-room")]
        public async Task<IActionResult> GetRooms()
        {
            return Ok(await _repo.GetRooms());
        }

        [HttpGet("get-messages/{customerID}")]
        public async Task<IActionResult> GetMessageOfRoom(string customerID)
        {
            return Ok(await _repo.GetMessageOfRoom(customerID));
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> AddMessage([FromForm] MessageVM message)
        {
            var mess = await _repo.AddMessage(message);

            //if (!message.IsCustomerSend)
            //{
            //    await _hub.Clients.All.SendAsync("ReceiveMessageAdmin", message);
            //}
            //else
            //{
            //    await _hub.Clients.All.SendAsync("ReceiveMessageCustomer", message);
            //}

            return Ok(mess);
        }
    }
}
