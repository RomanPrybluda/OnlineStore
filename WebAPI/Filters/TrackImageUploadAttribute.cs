using BSExpPhotos.Interfaces;
using BSExpPhotos.Metadata;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters;

public class TrackImageUploadAttribute : IAsyncActionFilter
{
    private readonly IImageInfoExtractor _imageInfoExtractor;
    private readonly ILogger<TrackImageUploadAttribute> _logger;
    private readonly IImageCleanupService _imageCleanupService;

    public TrackImageUploadAttribute(IImageInfoExtractor imageInfoExtractor, ILogger<TrackImageUploadAttribute> logger,
        IImageCleanupService imageCleanupService)
    {
        _imageInfoExtractor = imageInfoExtractor;
        _logger = logger;
        _imageCleanupService = imageCleanupService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executedContext = await next();
        // Получаем возвращённый результат
        if (executedContext.Result is not ObjectResult objectResult || objectResult.Value is null)
            return;

        var returnedEntity = objectResult.Value;
        var entityType = returnedEntity.GetType().Name; // Например: "Product" или "Promotion"
        _logger.LogWarning("EntityType: {entityType}, ReturnedEntity: {returnedEntity}", entityType, returnedEntity.ToString());
        var metadata = new ImageMetadata
        {
            EntityType = entityType switch
            {
                "ProductDTO" => Photo.EntityType.Product,
                "PromotionDTO" => Photo.EntityType.Promotion,
                "CategoryDTO" => Photo.EntityType.Category,
                _ => Photo.EntityType.None
            },
            CreatedAt = DateTime.UtcNow,
            FileNames = await _imageInfoExtractor.ExtractImageFileNames(returnedEntity)
        };


        var idProperty = returnedEntity.GetType().GetProperty("Id");
        var entityId = idProperty?.GetValue(returnedEntity);

        metadata.EntityId = entityId switch
        {
            Guid g => g,
            string s when Guid.TryParse(s, out var parsed) => parsed,
            _ => Guid.Empty
        };

        if (metadata.FileNames.Count == 0)
        {
            _logger.LogWarning("No image file names found for {EntityType} with ID {EntityId}",
                metadata.EntityType, metadata.EntityId);
            return;
        }


        var service = context.HttpContext.RequestServices.GetRequiredService<IImageUploadMetadataService>();

        foreach (var fileName in metadata.FileNames)
        {
            await service.SaveImageInfoToDb(fileName, metadata.EntityType,
                metadata.EntityId, metadata.CreatedAt);

            _logger.LogInformation(
                "Image metadata saved for {EntityType} with ID {EntityId} with fileName {fileName}",
                metadata.EntityType, metadata.EntityId, fileName);
        }

        _logger.LogInformation("Finished processing image upload metadata for {EntityType} with ID {EntityId}",
            metadata.EntityType, metadata.EntityId);
        
        await _imageCleanupService.MarkRemovedImagesAsDeletedAsync(metadata.EntityId, metadata.EntityType,
            metadata.FileNames);

    }
}