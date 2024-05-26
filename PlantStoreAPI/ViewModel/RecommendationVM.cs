using PlantStoreAPI.Model;

namespace PlantStoreAPI.ViewModel
{
    public class RecommendationVM
    {
        public float Score { get; set; }
        public Product Product { get; set; }
        public RecommendationVM(float score, Product product)
        {
            Score = score;
            Product = product;
        }
    }
}
