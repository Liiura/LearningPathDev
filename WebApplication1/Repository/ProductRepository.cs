using LearningPathDev.DatabaseContext;
using LearningPathDev.Interfaces;
using LearningPathDev.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace LearningPathDev.Repository
{
    public class ProductRepository : IProduct
    {
        private readonly ProductsContext _DbProducts;
        public ProductRepository(ProductsContext _dbProduts)
        {
            _DbProducts = _dbProduts;
        }
        public async Task<bool> CreateProduct(Product product)
        {
            using var context = _DbProducts;
            product.Id = Guid.NewGuid();
            await context.Product.AddAsync(product);
            if (await Save())
            {
                return true;
            }
            return false;
        }
        public async Task<List<Product>> GetllProducts()
        {
            var products = await _DbProducts.Product.ToListAsync();
            return products;
        }

        public async Task<bool> Save()
        {
            using var context = _DbProducts;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> GetProductByDescription(string description)
        {
            var products = await GetllProducts();
            var productByDesc = products.Where(x => x.Description == description).FirstOrDefault();
            return productByDesc;
        }
        public async Task<Product> GetProductById(Guid Id)
        {
            var products = await GetllProducts();
            var productByDesc = products.Where(x => x.Id == Id).FirstOrDefault();
            return productByDesc;
        }

        public async Task<bool> DeleteProduct(Guid Id)
        {
            var product = await GetProductById(Id);
            _DbProducts.Remove(product);
            if (await Save())
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateProduct(Guid Id, Product productUpdate)
        {
            var productSearch = await GetProductById(Id);
            productSearch.Description = productUpdate.Description;
            productSearch.Price = productUpdate.Price;
            productSearch.ProductState = productUpdate.ProductState;
            productSearch.PurchaseDate = productUpdate.PurchaseDate;
            productSearch.Quantity = productUpdate.Quantity;
            if (await Save())
            {
                return true;
            }
            return false;
        }
    }
}
