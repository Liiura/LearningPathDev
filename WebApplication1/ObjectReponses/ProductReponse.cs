using LearningPathDev.Models;
using System.Collections.Generic;

namespace LearningPathDev.ObjectReponses
{
    public class ProductReponse
    {
        public int StatusCode { get; set; }
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public string Error { get; set; }

    }
}
