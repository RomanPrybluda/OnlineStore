using DAL;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ProductService
    {

        private readonly OnlineStoreDbContext _context;
        private readonly ImageService _imageService;

        public ProductService(OnlineStoreDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
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

            if (filter.MinRating.HasValue)
                query = query.Where(p => p.Rating >= filter.MinRating.Value);

            if (filter.MaxRating.HasValue)
                query = query.Where(p => p.Rating <= filter.MaxRating.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(p => p.IsActive == filter.IsActive.Value);

            if (filter.IsSugarFree.HasValue)
                query = query.Where(p => p.IsSugarFree == filter.IsSugarFree.Value);

            if (filter.IsGlutenFree.HasValue)
                query = query.Where(p => p.IsGlutenFree == filter.IsGlutenFree.Value);

            if (filter.SortBy.HasValue)
            {
                bool isAscending = filter.SortDirection == SortDirection.Asc;

                query = filter.SortBy switch
                {
                    ProductSortBy.Price => isAscending
                        ? query.OrderBy(p => p.Price)
                        : query.OrderByDescending(p => p.Price),

                    ProductSortBy.Rating => isAscending
                        ? query.OrderBy(p => p.Rating)
                        : query.OrderByDescending(p => p.Rating),

                    _ => query
                };
            }

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

        public async Task<List<PopularProductDTO>> GetPopularProductsAsync(int count = 10)
        {
            var products = await _context.Products
                .Include(p => p.FavoritedByUsers)
                .Include(p => p.Reviews)
                .Where(p => p.IsActive)
                .ToListAsync();

            var popularProducts = products
                .Select(p => new
                {
                    Product = p,
                    PopularityScore =
                        (p.Views * 0.4) +
                        (p.FavoritedByUsers.Count * 0.3) +
                        (p.Reviews.Count * 0.2) +
                        (p.Rating * 0.1)
                })
                .OrderByDescending(p => p.PopularityScore)
                .Take(count)
                .Select(p => PopularProductDTO.FromProduct(p.Product, p.PopularityScore))
                .ToList();

            return popularProducts;
        }

        public async Task<List<BestSellerProductDTO>> GetBestSellerProductsAsync(int count = 10)
        {
            var bestSellers = await _context.Products
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.SoldQuantity)
                .Take(count)
                .ToListAsync();

            return bestSellers
                .Select(BestSellerProductDTO.FromProduct)
                .ToList();
        }

        public async Task<List<ProductDTO>> GetLatestProductsAsync(int count = 10)
        {
            var latestProducts = await _context.Products
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();

            return latestProducts
                .Select(ProductDTO.FromProduct)
                .ToList();
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

        public async Task<ProductDTO> UploadProductImagesAsync(Guid id, AddProductImageDTO request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new CustomException(CustomExceptionType.NotFound, $"Product with ID {id} not found.");

            string mainImageBaseName = string.Empty;
            if (request.MainProductImage != null)
                mainImageBaseName = await _imageService.UploadImageAsync(request.MainProductImage);

            List<string> imageBaseNames = new();
            if (request.ProductImages != null)
                imageBaseNames = await _imageService.UploadMultipleImagesAsync(request.ProductImages);

            request.AddProductImage(product, mainImageBaseName, imageBaseNames);

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return ProductDTO.FromProduct(product);
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

            string mainImageBaseName = string.Empty;
            if (request.MainProductImage != null)
                mainImageBaseName = await _imageService.UploadImageAsync(request.MainProductImage);

            List<string> imageBaseNames = new();
            if (request.ProductImages != null)
                imageBaseNames = await _imageService.UploadMultipleImagesAsync(request.ProductImages);

            request.UpdateProduct(product, mainImageBaseName, imageBaseNames);

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
