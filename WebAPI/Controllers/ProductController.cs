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
            var products = await _productService.GetProductsListAsync();
            return Ok(products);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetProductByIdAsyncAsync([Required] Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
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
