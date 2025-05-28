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
        var routeValues = context.RouteData.Values;
        string? controller = routeValues["controller"]?.ToString();
        string? idStr = routeValues["id"]?.ToString();
        var isPut = context.HttpContext.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase);
        
        if (!isPut || string.IsNullOrEmpty(controller) || !Guid.TryParse(idStr, out Guid id))
        {
            await next();
            return;
        }
        
        var metadataOldObject = await HandleUpdateAsyncFirst(controller, id);
        
        var executedContext = await next();
        // Получаем возвращённый результат
        if (executedContext.Result is not ObjectResult objectResult || objectResult.Value is null)
            return;

        var returnedEntity = objectResult.Value;
        var entityType = metadataOldObject.EntityType; // Например: "Product" или "Promotion"
        _logger.LogWarning("EntityType: {entityType}, ReturnedEntity: {returnedEntity}", entityType, returnedEntity.ToString());
        var metadata = new ImageMetadata
        {
            EntityType = entityType,
            CreatedAt = DateTime.UtcNow, // here we go
            FileNames = await _imageInfoExtractor.ExtractImageFileNames(TODO, TODO)
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
    
    private async Task<ImageMetadata> HandleUpdateAsyncFirst(string? controller, Guid id)
    {
        var entityType = controller?.ToLower() switch
        {
            "product" => Photo.EntityType.Product,
            "promotions" => Photo.EntityType.Promotion,
            "category" => Photo.EntityType.Category,
            _ => Photo.EntityType.None
        };
        _logger.LogInformation("entity type for {controller}/{id}", controller, id);

        if (entityType == Photo.EntityType.None)
        {
            _logger.LogWarning("No entity type found for {controller}/{id}", controller, id);
            return new ImageMetadata()
            {
                EntityType = entityType,
            };
        }


        try
        {
            var metadataOldObject = new ImageMetadata
            {
                EntityType = entityType,
                CreatedAt = DateTime.UtcNow,
                FileNames =  await _imageInfoExtractor.ExtractImageFileNames(id, entityType),
                EntityId = id
            };
                

            if (metadataOldObject.FileNames.Count == 0)
            {
                _logger.LogInformation("No photos found for updating {controller}/{id}", controller, id);
            }
                
            return metadataOldObject;
        }
        catch (Exception ex)
        {
            // Log the error or handle it as needed
            _logger.LogError(ex, " entity {id} in controller {Controller}", 
                id, controller);
            throw;
        }
    }

}