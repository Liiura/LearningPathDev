using LearningPathDev.Models.Common;

namespace LearningPathDev.Models.DTO
{
    public class ProductDTO
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public double? Price { get; set; }
        public ProductStateOptions? ProductState { get; set; }
    }
}
