using AutoMapper;
using LearningPathDev.DatabaseContext;
using LearningPathDev.Interfaces;
using LearningPathDev.Models;
using LearningPathDev.Models.DTO;
using LearningPathDev.ObjectReponses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LearningPathDev.Repository
{
    public class ProductRepository : IProduct
    {
        private readonly ProductsContext _DbProducts;
        private readonly IMapper _Mapper;
        public ProductRepository(ProductsContext _dbProduts, IMapper mapper)
        {
            _DbProducts = _dbProduts;
            _Mapper = mapper;
        }
        public async Task<ProductReponse> CreateProduct(Product product)
        {
            try
            {
                product.Id = Guid.NewGuid();
                product.PurchaseDate = DateTimeOffset.Now.ToUniversalTime();
                await _DbProducts.Product.AddAsync(product);
                if (await Save())
                {
                    return new ProductReponse
                    {
                        TransactionState = true,
                        StatusCode = 201
                    };
                }
                return new ProductReponse
                {
                    TransactionState = false,
                    StatusCode = 500,
                    Error = "Internal error"
                };
            }
            catch (Exception ex)
            {

                return new ProductReponse
                {
                    TransactionState = false,
                    StatusCode = 500,
                    Error = ex.ToString()
                };
            }
        }
        public async Task<ProductReponse> GetllProducts()
        {
            try
            {
                var products = await _DbProducts.Product.ToListAsync();
                return new ProductReponse
                {
                    Products = products,
                    StatusCode = 200,
                    TransactionState = true
                };
            }
            catch (Exception ex)
            {

                return new ProductReponse
                {
                    StatusCode = 500,
                    Error = ex.ToString(),
                    TransactionState = false
                };
            }
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
                var AllProducts = await GetllProducts();
                if (Id != null && Id != Guid.Empty)
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
                    var productsFiltered = AllProducts.Products.Where(x => x.Description.Contains(description)).ToList();
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
            var AllProducts = await GetllProducts();
            var productByDesc = AllProducts.Products.Where(x => x.Id == Id).FirstOrDefault();
            return productByDesc;
        }

        public async Task<ProductReponse> DeleteProduct(Guid Id)
        {
            try
            {
                var product = await GetProductById(Id);
                if (product == null)
                {
                    return new ProductReponse
                    {
                        StatusCode = 404,
                        TransactionState = false,
                        Error = "Resource not found"
                    };
                }
                _DbProducts.Remove(product);
                if (await Save())
                {
                    return new ProductReponse
                    {
                        StatusCode = 201,
                        TransactionState = true
                    };
                }
                return new ProductReponse
                {
                    StatusCode = 500,
                    TransactionState = false,
                    Error = "Internal error"
                };
            }
            catch (Exception ex)
            {
                return new ProductReponse
                {
                    StatusCode = 500,
                    TransactionState = false,
                    Error = ex.ToString()
                };
            }
        }
        public async Task<ProductReponse> UpdateProduct(Guid Id, ProductDTO productUpdate)
        {
            try
            {
                var productSearch = await GetProductById(Id);
                if (productSearch == null)
                {
                    return new ProductReponse
                    {
                        StatusCode = 404,
                        TransactionState = false,
                        Error = "Resource not found"
                    };
                }
                productSearch = _Mapper.Map(productUpdate, productSearch);
                if (await Save())
                {
                    return new ProductReponse
                    {
                        StatusCode = 201,
                        TransactionState = true
                    };
                }
                return new ProductReponse
                {
                    StatusCode = 500,
                    TransactionState = false,
                    Error = "Internal error"
                };
            }
            catch (Exception ex)
            {
                return new ProductReponse
                {
                    StatusCode = 500,
                    TransactionState = false,
                    Error = ex.ToString()
                };
            }
        }
    }
}
