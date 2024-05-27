using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IChatRepo
    {
        Task<List<Message>> GetMessageOfRoom(string customerID);
        Task<Message> AddMessage(MessageVM message);
        //Task<List<string>> GetAdminsId();
        Task<List<ChatRoom>> GetRooms();
    }
}
