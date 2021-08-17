using LearningPathDev.Models;
using LearningPathDev.Models.DTO;
using LearningPathDev.ObjectReponses;
using System;
using System.Threading.Tasks;

namespace LearningPathDev.Interfaces
{
    public interface IProduct
    {
        Task<ProductReponse> CreateProduct(Product product);
        Task<ProductReponse> GetllProducts();
        Task<ProductReponse> GetProductByFilter(string description, Guid Id);
        Task<ProductReponse> DeleteProduct(Guid Id);
        Task<ProductReponse> UpdateProduct(Guid Id, ProductDTO productDTO);
    }
}
