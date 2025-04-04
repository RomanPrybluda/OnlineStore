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

        public async Task<PagedResponseDTO<ProductDTO>> GetProductsListAsync(ProductFilterDTO filter)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
                query = query.Where(p => p.Name.ToLower().Contains(filter.SearchQuery.ToLower()));

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            if (filter.SortBy.HasValue)
            {
                bool isAscending = filter.SortDirection == SortDirection.Asc;

                query = filter.SortBy switch
                {
                    ProductSortBy.Price => isAscending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                    ProductSortBy.Rating => isAscending ? query.OrderBy(p => p.Rating) : query.OrderByDescending(p => p.Rating),
                    _ => query
                };
            }

            if (filter.IsActive.HasValue)
                query = query.Where(p => p.IsActive == filter.IsActive.Value);

            int totalItems = await query.CountAsync();

            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
            {
                int skip = (filter.PageNumber.Value - 1) * filter.PageSize.Value;
                int take = filter.PageSize.Value;

                var products = await query.Skip(skip).Take(take).ToListAsync();

                return new PagedResponseDTO<ProductDTO>(
                    products.Select(ProductDTO.FromProduct),
                    totalItems,
                    skip,
                    take
                );
            }

            var allProducts = await query.ToListAsync();

            return new PagedResponseDTO<ProductDTO>(
                allProducts.Select(ProductDTO.FromProduct),
                totalItems,
                0,
                totalItems
            );
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
