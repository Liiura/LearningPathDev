using LearningPathDev.DatabaseContext;
using LearningPathDev.Interfaces;
using LearningPathDev.Models;
using LearningPathDev.ObjectReponses;
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

        public async Task<ProductReponse> GetProductByFilter(string description, Guid Id)
        {
            ProductReponse productReponse = null;
            try
            {
                var products = await GetllProducts();
                if (Id != null || Id != Guid.Empty || !string.IsNullOrEmpty(description))
                {
                    var productFiltered = await GetProductById(Id);
                    productReponse = new ProductReponse
                    {
                        StatusCode = 200,
                        Product = productFiltered
                    };
                }
                else if (!string.IsNullOrEmpty(description))
                {
                    var productsFiltered = products.Where(x => x.Description.Contains(description)).ToList();
                    if (productsFiltered.Count == 0)
                    {
                        productReponse = new ProductReponse
                        {
                            StatusCode = 404,
                            Error = "Resource not found"
                        };
                    }
                    else
                    {
                        productReponse = new ProductReponse
                        {
                            StatusCode = 200,
                            Products = productsFiltered
                        };
                    }
                }
                else
                {
                    productReponse = new ProductReponse
                    {
                        StatusCode = 404,
                        Error = "Resource not found"
                    };
                }
            }
            catch (Exception ex)
            {
                productReponse = new ProductReponse
                {
                    StatusCode = 500,
                    Error = "" + ex.ToString()
                };
            }
            return productReponse;

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
            if (await Save())
            {
                return true;
            }
            return false;
        }
    }
}
