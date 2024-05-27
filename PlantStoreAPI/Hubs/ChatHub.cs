using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using PlantStoreAPI.Model;

namespace PlantStoreAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} has joined to ChatHub");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.ConnectionId} has left the ChatHub");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
