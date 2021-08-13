using LearningPathDev.Models.Common;
using System;


namespace LearningPathDev.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double? Price { get; set; }
        public DateTimeOffset? PurchaseDate { get; set; }
        public ProductStateOptions? ProductState { get; set; }
    }
}
