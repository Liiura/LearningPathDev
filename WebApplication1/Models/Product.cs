using System;


namespace LearningPathDev.Models
{
    public class Product
    {
        private bool productState;

        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public double? Price { get; set; }
        public DateTimeOffset? PurchaseDate { get; set; }
        public bool ProductState { get => productState; set => productState = value; }
        public int? Quantity { get; set; }
    }
}
