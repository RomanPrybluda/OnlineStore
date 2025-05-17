using BSExpPhotos.Interfaces;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BSExpPhotos.Services;

public class ImageInfoExtractor : IImageInfoExtractor
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILogger<ImageInfoExtractor> _logger;

    public ImageInfoExtractor(OnlineStoreDbContext dbContext, ILogger<ImageInfoExtractor> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    
    public async Task<List<string>> ExtractImageFileNames(object entity)
    {
        return entity switch
        {
            ProductDTO p => await ExtractFromProductDTO(p),
            PromotionDTO promo => await ExtractFromPromotionDTO(promo),
            CategoryDTO cat => await ExtractFromCategoryDTO(cat),
            _ => new List<string>()
        };
    }

    private async Task<List<string>> ExtractFromProductDTO(ProductDTO productDto)
    {
        var result = new List<string>();

        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productDto.Id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found.", productDto.Id);
            return result;
        }

        if (!string.IsNullOrWhiteSpace(product.MainImageBaseName))
            result.Add(product.MainImageBaseName);

        if (product.ImageBaseNames?.Any() == true)
            result.AddRange(product.ImageBaseNames.Where(x => !string.IsNullOrWhiteSpace(x)));

        return result;
    }

    private async Task<List<string>> ExtractFromPromotionDTO(PromotionDTO promotionDto)
    {
        var result = new List<string>();

        var promotion = await _dbContext.Promotions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == promotionDto.Id);

        if (promotion == null)
        {
            _logger.LogWarning("Promotion with ID {PromotionId} not found.", promotionDto.Id);
            return result;
        }

        if (!string.IsNullOrWhiteSpace(promotion.ImageBannerName))
            result.Add(promotion.ImageBannerName);

        return result;
    }

    private async Task<List<string>> ExtractFromCategoryDTO(CategoryDTO categoryDto)
    {
        var result = new List<string>();

        var category = await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryDto.Id);

        if (category == null)
        {
            _logger.LogWarning("Category with ID {CategoryId} not found.", categoryDto.Id);
            return result;
        }

        if (!string.IsNullOrWhiteSpace(category.ImageBaseName))
            result.Add(category.ImageBaseName);

        return result;
    }
}
