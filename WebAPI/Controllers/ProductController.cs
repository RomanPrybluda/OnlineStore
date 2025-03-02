using Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{

    [ApiController]
    [Produces("application/json")]
    [Route("products")]

    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProductsAsyncAsync()
        {
            var Products = await _productService.GetProductsListAsync();
            return Ok(Products);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetProductByIdAsyncAsync([Required] Guid id)
        {
            var Product = await _productService.GetProductByIdAsync(id);
            return Ok(Product);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<ActionResult> DeleteShipAsync([Required] Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

    }
}
