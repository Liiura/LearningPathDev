using LearningPathDev.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningPathDev.Interfaces
{
    public interface IProduct
    {
        Task<bool> CreateProduct(Product product);
        Task<List<Product>> GetllProducts();
        Task<Product> GetProductByDescription(string description);
        Task<bool> DeleteProduct(Guid Id);
        Task<bool> UpdateProduct(Guid Id, Product productUpdate);
    }
}
