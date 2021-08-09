using LearningPathDev.DatabaseContext;
using LearningPathDev.Interfaces;
using LearningPathDev.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace LearningPathDev.Repository
{
    public class ProductRepository : IProduct
    {
        private readonly ProductsContext _dbProducts;
        public ProductRepository(ProductsContext _dbProduts)
        {
            this._dbProducts = _dbProduts;
        }
        public async Task<bool> CreateProduct(Product product)
        {
            using var context = _dbProducts;
            product.Id = Guid.NewGuid();
            await context.Product.AddAsync(product);
            if (await Save())
            {
                return true;
            }
            return false;
        }

        public Task<List<Product>> GetllProducts()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            using var context = _dbProducts;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
