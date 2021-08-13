﻿using LearningPathDev.Interfaces;
using LearningPathDev.Models;
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
        public ProductController(IProduct Iproduct) => _IProduct = Iproduct;
        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var AllProducts = await _IProduct.GetllProducts();
            return Ok(AllProducts);
        }

        // GET api/<ProductController>/5
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> GetProductWithFilter(string description, Guid Id)
        {
            if (string.IsNullOrEmpty(description) && Id == Guid.Empty)
            {
                return StatusCode(400, "bad request");
            }

            var response = await _IProduct.GetProductByFilter(description, Id);
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
        public async Task<IActionResult> CreateProductController([FromBody] Product product)
        {
            if (product == null)
            {
                return StatusCode(400, "You need provide a valid data for update a resource");
            }
            bool isInsert = await _IProduct.CreateProduct(product);
            string message;
            if (isInsert)
            {
                message = "Product was inserted";
                return StatusCode(201, new { message });
            }
            message = "Internal server error";
            return StatusCode(500, new { message });
        }

        // PUT api/<ProductController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Guid Id, [FromBody] Product productUpdate)
        {
            if (Id == null || Id == Guid.Empty)
            {
                return StatusCode(400, "You need provide a valid identification for delete the resource");
            }
            if (productUpdate == null)
            {
                return StatusCode(400, "You need provide a valid data for update a resource");
            }
            bool isUpdated = await _IProduct.UpdateProduct(Id, productUpdate);
            string message;
            if (isUpdated)
            {
                message = $"Product with Id = {Id} was updated";
                return Ok(new { message });
            }
            message = "Internal server error";
            return StatusCode(500, new { message });
        }

        // DELETE api/<ProductController>/5
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(Guid Id)
        {
            if (Id == null || Id == Guid.Empty)
            {
                return StatusCode(400, "You need provide a valid identification for delete the resource");
            }
            bool isDeleted = await _IProduct.DeleteProduct(Id);
            string message;
            if (isDeleted)
            {
                message = $"Product with Id = {Id} was deleted";
                return Ok(new { message });
            }
            message = "Internal server error";
            return StatusCode(500, new { message });
        }
    }
}
