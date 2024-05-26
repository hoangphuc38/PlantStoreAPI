using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Data;
using PlantStoreAPI.Model;
using PlantStoreAPI.Repositories;
using PlantStoreAPI.StaticServices;
using PlantStoreAPI.ViewModel;

namespace PlantStoreAPI.Services
{
    public class FeedbackRepo : IFeedbackRepo
    {
        public DataContext _context;
        public FeedbackRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Feedback>> GetAll(string productID)
        {
            var feedbackList = await _context.Feedbacks.Where(c => c.ProductID == productID).ToListAsync();

            if (feedbackList ==  null)
            {
                feedbackList = new List<Feedback>();
            }

            foreach (var feedback in feedbackList)
            {
                var product = await _context.Products.FindAsync(feedback.ProductID);
                var customer = await _context.Customers.FindAsync(feedback.CustomerID);

                if (product != null)
                {
                    feedback.Product = product;
                }

                if (customer != null)
                {
                    feedback.Customer = customer;
                }
            }

            return feedbackList;
        }
        public async Task<List<Feedback>> GetAllByCustomer(string customerID)
        {
            var feedbackList = await _context.Feedbacks.Where(c => c.CustomerID == customerID).ToListAsync();

            if (feedbackList == null)
            {
                feedbackList = new List<Feedback>();
            }

            foreach (var feedback in feedbackList)
            {
                var product = await _context.Products.FindAsync(feedback.ProductID);
                var customer = await _context.Customers.FindAsync(feedback.CustomerID);

                if (product != null)
                {
                    feedback.Product = product;
                }

                if (customer != null)
                {
                    feedback.Customer = customer;
                }
            }

            return feedbackList;
        }
        public async Task<Feedback> Add(FeedbackVM feedbackVM)
        {
            var feedback = new Feedback
            {
                CustomerID = feedbackVM.CustomerID,
                ProductID = feedbackVM.ProductID,
                Comment = feedbackVM.Comment,
                Point = feedbackVM.Point,
                FeedbackTime = DateTime.Now,
            };

            if (feedbackVM.ImageFeedback != null)
            {
                feedback.ImageFeedback = await UploadImage.Instance.UploadAsync(feedbackVM.ImageFeedback);
            }

            var product = await _context.Products.FindAsync(feedbackVM.ProductID);
            var customer = await _context.Customers.FindAsync(feedbackVM.CustomerID);
            
            if (product != null)
            {
                feedback.Product = product;                             
            }
            
            if (customer != null)
            {
                feedback.Customer = customer;
            }

            var orders = await _context.OrderDetails.Where(d => d.OrderID == feedbackVM.OrderID).ToListAsync();
            foreach (var order in orders)
            {
                if (order.ProductID == feedbackVM.ProductID)
                {
                    order.didFeedback = true;
                }
            }

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            if (product != null)
            {
                if (product.ProductID != null)
                {
                    product.ReviewPoint = await GetAvgPoint(product.ProductID);
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
            }

            var pdID = int.Parse(feedback.ProductID.Substring(2));
            var csID = int.Parse(feedback.CustomerID.Substring(2));
            var rating = feedback.Point;
            _context.Database.ExecuteSqlRaw($"IF EXISTS (SELECT * FROM DatasetUserAndFeedback WHERE ProductID = '{pdID}' AND CustomerID = '{csID}') " +
                $"BEGIN UPDATE DatasetUserAndFeedback SET Rating = {rating} WHERE ProductID = '{pdID}' AND " +
                $"CustomerID = '{csID}';END " +
                $"ELSE BEGIN INSERT INTO DatasetUserAndFeedback (CustomerID, ProductID, Rating)" +
                $"VALUES ('{csID}', '{pdID}', {rating});END");

            return feedback;
        }
        public async Task<double> GetAvgPoint(string productID)
        {
            return await _context.Feedbacks.Where(f => f.ProductID == productID)
                .Select(f => f.Point)
                .AverageAsync();
        }
        public async Task<Feedback> Update(FeedbackVM feedbackVM)
        {
            var feedback = await _context.Feedbacks
                .FindAsync(feedbackVM.CustomerID, feedbackVM.ProductID);

            if (feedback == null)
            {
                throw new KeyNotFoundException();
            }

            feedback.Comment = feedbackVM.Comment;
            feedback.Point = feedbackVM.Point;

            if (feedbackVM.ImageFeedback != null)
            {
                feedback.ImageFeedback = await UploadImage.Instance.UploadAsync(feedbackVM.ImageFeedback);
            }
            
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();

            var product = await _context.Products.FindAsync(feedbackVM.ProductID);

            if (product != null)
            {
                if (product.ProductID != null)
                {
                    product.ReviewPoint = await GetAvgPoint(product.ProductID);
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
            }

            return feedback;
        }
    }
}
