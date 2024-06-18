using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Hubs;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.StaticServices;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Services
{
    public class ChatRepo : IChatRepo
    {
        public DataContext _context;
        private readonly IHubContext<ChatHub> _hub;
        public ChatRepo(DataContext context, IHubContext<ChatHub> hub)
        {
            _context = context;
            _hub = hub;
        }
        public async Task<List<Message>> GetMessageOfRoom(string customerID)
        {
            var room = await _context.ChatRooms.Where(r => r.CustomerID == customerID).FirstOrDefaultAsync();

            if (room != null)
            {
                return await _context.Messages.OrderByDescending(m => m.SendTime)
                                 .Where(m => m.RoomID == room.RoomID)
                                 .ToListAsync();
            }
            else
            {
                return new List<Message>();
            }           
        }
        public async Task<Message> AddMessage(MessageVM message)
        {
            var newMessage = new Message
            {
                Content = message.Content,
                SendTime = DateTime.Now,
                IsCustomerSend = message.IsCustomerSend,
            };

            var room = await _context.ChatRooms
                .Where(m => m.CustomerID == message.CustomerID)
                .FirstOrDefaultAsync();

            if (room == null)
            {
                throw new KeyNotFoundException();
            }

            newMessage.Room = room;
            if (message.Image != null)
            {
                newMessage.Image = await UploadImage.Instance.UploadAsync(message.Image);
            }
            else
            {
                newMessage.Image = "";
            }
          
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            Console.WriteLine(newMessage);

            if (!message.IsCustomerSend)
            {
                await _hub.Clients.All.SendAsync("ReceiveMessageAdmin", newMessage);
            }
            else
            {
                await _hub.Clients.All.SendAsync("ReceiveMessageCustomer", newMessage);
            }

            return newMessage;
        }
        public async Task<List<ChatRoom>> GetRooms()
        {
            var rooms = await _context.Messages.OrderByDescending(r => r.SendTime).Select(r => r.RoomID).Distinct().ToListAsync();
            var result = new List<ChatRoom>();

            foreach (var room in rooms)
            {
                var roomInfo = await _context.ChatRooms.Where(r => r.RoomID == room).FirstOrDefaultAsync();

                if (roomInfo != null)
                {
                    result.Add(new ChatRoom
                    {
                        RoomID = room,
                        CustomerID = roomInfo.CustomerID,
                        Customer = await _context.Customers.FindAsync(roomInfo.CustomerID)
                    });
                }
            }

            var roomList = await _context.ChatRooms.ToListAsync();
            foreach (var room in roomList)
            {
                var roomMessage = await _context.Messages
                                                .Where(c => c.RoomID == room.RoomID)
                                                .FirstOrDefaultAsync();
                if (roomMessage == null)
                {
                    result.Add(new ChatRoom
                    {
                        RoomID = room.RoomID,
                        CustomerID = room.CustomerID,
                        Customer = await _context.Customers.FindAsync(room.CustomerID)
                    });
                }
            }

            return result;
        }
    }
}
