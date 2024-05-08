using PlantStoreAPI.Model;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Repositories
{
    public interface IFeedbackRepo
    {
        Task<List<Feedback>> GetAll(string productID);
        Task<List<Feedback>> GetAllByCustomer(string customerID);
        Task<double> GetAvgPoint(string productID);
        Task<Feedback> Add(FeedbackVM feedback);
        Task<Feedback> Update(FeedbackVM feedback);
    }
}
