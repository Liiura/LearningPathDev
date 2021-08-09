using LearningPathDev.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningPathDev.Interfaces
{
    public interface IProduct
    {
        Task<bool> CreateProduct(Product product);
        Task<List<Product>> GetllProducts();
    }
}
