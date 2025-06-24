using BSExpPhotos.Interfaces;
using DAL;
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
    
    
    public async Task<List<string>> ExtractImageFileNames(Guid entityIdGuid, Photo.EntityType  type)
    {
        return type switch
        {
            Photo.EntityType.Product  => await ExtractFromProductDto(entityIdGuid),
            Photo.EntityType.Promotion => await ExtractFromPromotionDto(entityIdGuid),
            Photo.EntityType.Category => await ExtractFromCategoryDto(entityIdGuid),
            _ => new List<string>()
        
        };
    }

    private async Task<List<string>> ExtractFromProductDto(Guid entityIdGuid)
    {
        var result = new List<string>();

        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == entityIdGuid);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {entityIdGuid} not found.", entityIdGuid);
            return result;
        }

        if (!string.IsNullOrWhiteSpace(product.MainImageBaseName))
            result.Add(product.MainImageBaseName);

        if (product.ImageBaseNames?.Any() == true)
            result.AddRange(product.ImageBaseNames.Where(x => !string.IsNullOrWhiteSpace(x)));

        return result;
    }

    private async Task<List<string>> ExtractFromPromotionDto(Guid entityIdGuid)
    {
        var result = new List<string>();

        var promotion = await _dbContext.Promotions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == entityIdGuid);

        if (promotion == null)
        {
            _logger.LogWarning("Promotion with ID {entityIdGuid} not found.", entityIdGuid);
            return result;
        }

        if (!string.IsNullOrWhiteSpace(promotion.ImageBannerName))
            result.Add(promotion.ImageBannerName);

        return result;
    }

    private async Task<List<string>> ExtractFromCategoryDto(Guid entityIdGuid)
    {
        var result = new List<string>();

        var category = await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == entityIdGuid);

        if (category == null)
        {
            _logger.LogWarning("Category with ID {entityIdGuid} not found.", entityIdGuid);
            return result;
        }

        if (!string.IsNullOrWhiteSpace(category.ImageBaseName))
            result.Add(category.ImageBaseName);

        return result;
    }
}
