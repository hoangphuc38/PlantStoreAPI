namespace PlantStoreAPI.ViewModel
{
    public class FeedbackVM
    {
        public string? OrderID { get; set; }
        public string? CustomerID { get; set; }
        public string? ProductID { get; set; }
        public string? Comment { get; set; }
        public int Point { get; set; }
        public IFormFile? ImageFeedback { get; set; }
    }
}
