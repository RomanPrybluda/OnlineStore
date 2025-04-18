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
        public async Task<ActionResult> GetAllProductsAsync([FromQuery] ProductFilterDTO filter)
        {
            var products = await _productService.GetProductsListAsync(filter);
            return Ok(products);
        }

        [HttpGet("popular")]
        public async Task<ActionResult> GetPopularProductsAsync()
        {
            var products = await _productService.GetPopularProductsAsync();
            return Ok(products);
        }

        [HttpGet("bestsellers")]
        public async Task<ActionResult> GetBestSellerProductsAsync()
        {
            var products = await _productService.GetBestSellerProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetProductByIdAsync([Required] Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductAsync([FromForm][FromBody][Required] CreateProductDTO request)
        {
            var product = await _productService.CreateProductAsync(request);
            return Ok(product);
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateProductAsync([Required] Guid id, [FromForm][FromBody][Required] UpdateProductDTO request)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, request);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteProductAsync([Required] Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

    }
}
