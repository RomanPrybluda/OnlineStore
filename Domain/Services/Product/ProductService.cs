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

        public async Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            var productById = await _context.Products.FindAsync(id);

            if (productById == null)
                throw new CustomException(CustomExceptionType.NotFound, $"No ProductById with ID {id}");

            var productDTO = ProductDTO.FromProduct(productById);

            return productDTO;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var ship = await _context.Products.FindAsync(id);

            if (ship is null)
                throw new CustomException(CustomExceptionType.NotFound, $"No shipById with {id} id");

            _context.Products.Remove(ship);
            await _context.SaveChangesAsync();
        }



    }
}
