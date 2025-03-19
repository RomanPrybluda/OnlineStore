using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ProductService
    {

        private readonly OnlineStoreDbContext _context;

        public ProductService(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsListAsync()
        {
            var products = await _context.Products.ToListAsync();

            if (products == null || !products.Any())
                throw new CustomException(CustomExceptionType.NotFound, "No Products");

            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var productDTO = ProductDTO.FromProduct(product);
                productDTOs.Add(productDTO);
            }

            return productDTOs;

        }

        public async Task<ProductByIdDTO> GetProductByIdAsync(Guid id)
        {
            var productById = await _context.Products.FindAsync(id);

            if (productById == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No Product with ID {id}");

            var productDTO = ProductByIdDTO.FromProduct(productById);

            return productDTO;
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO request)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Sku == request.Sku);
            if (existingProduct != null)
                throw new CustomException(CustomExceptionType.ProductAlreadyExists, $"Product with SKU {request.Sku} already exists.");

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {request.CategoryId}");

            var product = CreateProductDTO.ToProduct(request);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var createdProduct = await _context.Products.FindAsync(product.Id);
            var productDTO = ProductDTO.FromProduct(createdProduct);

            return productDTO;
        }

        public async Task<ProductDTO> UpdateProductAsync(Guid id, UpdateProductDTO request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Product with ID {id} not found.");

            var existingProductWithSku = await _context.Products.FirstOrDefaultAsync(p => p.Sku == request.Sku && p.Id != id);

            if (existingProductWithSku != null)
                throw new CustomException(CustomExceptionType.ProductAlreadyExists, $"Product with SKU {request.Sku} already exists.");

            var category = await _context.Categories.FindAsync(request.CategoryId);

            if (category == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No category found with ID {request.CategoryId}");

            request.UpdateProduct(product);

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return ProductDTO.FromProduct(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var ship = await _context.Products.FindAsync(id);

            if (ship is null)
                throw new CustomException(CustomExceptionType.NotFound, $"No Product with {id} id");

            _context.Products.Remove(ship);
            await _context.SaveChangesAsync();
        }

    }
}
