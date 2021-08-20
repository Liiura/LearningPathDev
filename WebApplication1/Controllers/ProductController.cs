using AutoMapper;
using LearningPathDev.Interfaces;
using LearningPathDev.Models;
using LearningPathDev.Models.DTO;
using LearningPathDev.ObjectReponses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LearningPathDev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _IProduct;
        private readonly IMapper _Mapper;
        public ProductController(IProduct Iproduct, IMapper mapper)
        {
            _IProduct = Iproduct;
            _Mapper = mapper;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var payload = await _IProduct.GetllProducts();
            if (payload.TransactionState)
            {
                return StatusCode(payload.StatusCode, payload.Products);
            }
            return StatusCode(payload.StatusCode, payload.Error);
        }

        // GET api/<ProductController>/5
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> GetProductWithFilter(string description, string Id)
        {
            if (string.IsNullOrEmpty(description) && Guid.Parse(Id) == Guid.Empty)
            {
                return StatusCode(400, "bad request");
            }
            ProductReponse response = new ProductReponse();
            if (Id == null)
            {
                response = await _IProduct.GetProductByFilter(description, Guid.Empty);

            }
            if (response.StatusCode == 200)
            {
                if (response.Products != null)
                {
                    return StatusCode(response.StatusCode, response.Products);
                }
                else if (response.Product != null)
                {
                    return StatusCode(response.StatusCode, response.Product);
                }
                else
                {
                    return StatusCode(404, "resource not found");
                }
            }
            else if (response.StatusCode == 404 || response.StatusCode == 500)
            {
                return StatusCode(response.StatusCode, response.Error);
            }
            return StatusCode(500, "internal error :c");
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> CreateProductController([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                return StatusCode(400, "You need provide a valid data for create a resource");
            }
            var product = _Mapper.Map<Product>(productDTO);
            var payload = await _IProduct.CreateProduct(product);
            string message;
            if (payload.TransactionState)
            {
                message = "Product was inserted";
                return StatusCode(payload.StatusCode, new { message });
            }
            return StatusCode(payload.StatusCode, new { payload.Error });
        }

        // PUT api/<ProductController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string Id, [FromBody] ProductDTO productDTO)
        {
            if (Guid.Parse(Id) == null || Guid.Parse(Id) == Guid.Empty)
            {
                return StatusCode(400, "You need provide a valid identification for update the resource");
            }
            if (productDTO == null)
            {
                return StatusCode(400, "You need provide a valid data for update a resource");
            }
            var payload = await _IProduct.UpdateProduct(Guid.Parse(Id), productDTO);
            string message;
            if (payload.TransactionState)
            {
                message = $"Product with Id = {Id} was updated";
                return StatusCode(payload.StatusCode, new { message });
            }
            return StatusCode(payload.StatusCode, new { payload.Error });
        }

        // DELETE api/<ProductController>/5
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string Id)
        {
            if (Guid.Parse(Id) == null || Guid.Parse(Id) == Guid.Empty)
            {
                return StatusCode(400, "You need provide a valid identification for delete the resource");
            }
            var payload = await _IProduct.DeleteProduct(Guid.Parse(Id));
            string message;
            if (payload.TransactionState)
            {
                message = $"Product with Id = {Id} was deleted";
                return StatusCode(payload.StatusCode, new { message });
            }
            return StatusCode(payload.StatusCode, new { payload.Error });
        }
    }
}
