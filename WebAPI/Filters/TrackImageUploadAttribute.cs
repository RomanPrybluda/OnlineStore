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

    public TrackImageUploadAttribute(IImageInfoExtractor imageInfoExtractor,
        ImageMetadata imageUploadMetadata, ILogger<TrackImageUploadAttribute> logger)
    {
        _imageInfoExtractor = imageInfoExtractor;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var executedContext = await next();
            // Получаем возвращённый результат
            if (executedContext.Result is ObjectResult objectResult && objectResult.Value != null)
            {
                var returnedEntity = objectResult.Value;
                var entityType = returnedEntity.GetType().Name; // Например: "Product" или "Promotion"
                var metadata = new ImageMetadata
                {
                    EntityType = entityType switch
                    {
                        "Product" => Photo.EntityType.Product,
                        "Promotion" => Photo.EntityType.Promotion,
                        "Category" => Photo.EntityType.Category,
                        _ => Photo.EntityType.None
                    },
                    CreatedAt = DateTime.UtcNow,
                    FileNames = _imageInfoExtractor.ExtractImageFileNames(returnedEntity)
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
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while tracking image upload metadata");
        }
    }
}