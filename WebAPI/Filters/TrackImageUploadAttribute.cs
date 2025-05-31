using System.Diagnostics;
using BSExpPhotos.Interfaces;
using BSExpPhotos.Metadata;
using DAL;
using Domain;
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
        var method = context.HttpContext.Request.Method;
        
       
        
        if ( (method != "PUT" && method != "POST")
             || string.IsNullOrEmpty(controller) || !Guid.TryParse(idStr, out Guid id))
        {
            _logger.LogWarning("method {method} not supported, controller {controller} and id {id}", 
                method, controller, idStr);
            await next();
            return;
        }
        
        // get info about filenames before update or post
        var metadataOldObject = await HandleUpdateAsyncFirst(controller, id);
        
      

        if (metadataOldObject.FileNames.Count == 0)
        {
            await next();
            return;
        }
        
        
        
        var executedContext = await next();
        
        
        // Получаем возвращённый результат
        if (executedContext.Result is not ObjectResult objectResult || objectResult.Value is null)
            return;

        var returnedEntity = objectResult.Value;
        var entityType = metadataOldObject.EntityType;
        
        // get id updated entity
        Guid idUpdatedObject = returnedEntity switch
        {
            PromotionDTO promotion => promotion.Id,
            CategoryDTO category => category.Id,
            ProductDTO product => product.Id,
            _ => Guid.Empty
        };

        if (idUpdatedObject.Equals(Guid.Empty))
        {
            _logger.LogWarning("ReturnedEntity: {returnedEntity} with null Guid.", returnedEntity);
            return;
        }
        
        
        
        _logger.LogInformation("EntityType: {entityType}, ReturnedEntity: {returnedEntity}, Guid: {idUpdatedObject}", 
            entityType, returnedEntity.ToString(), idUpdatedObject);
        // get info about entity (filenames) after update
        var metadataUpdatedEntity = new ImageMetadata
        {
            EntityType = entityType,
            CreatedAt = DateTime.UtcNow, // here we go
            EntityId = idUpdatedObject,
            FileNames = await _imageInfoExtractor.ExtractImageFileNames(idUpdatedObject, entityType)
        };
        

        if (metadataUpdatedEntity.FileNames.Count == 0)
        {
            _logger.LogWarning("No image file names found for {EntityType} with ID {EntityId}",
                metadataUpdatedEntity.EntityType, metadataUpdatedEntity.EntityId);
            return;
        }
        
        var toDelete = GetFilesToDelete(metadataOldObject, metadataUpdatedEntity);

        var uploadMetadataService = context.HttpContext.RequestServices.GetRequiredService<IImageUploadMetadataService>();

        foreach (var fileName in toDelete)
        {
            await uploadMetadataService.SaveImageInfoToDb(fileName, entityType, metadataOldObject.EntityId,
                metadataOldObject.CreatedAt);

            _logger.LogInformation(
                "Image metadata saved for {EntityType} with ID {EntityId} with fileName {fileName}",
                metadataOldObject.EntityType, metadataOldObject.EntityId, fileName);
        }

        _logger.LogInformation("Finished processing image upload metadata for {EntityType} with ID {EntityId}",
            metadataOldObject.EntityType, metadataOldObject.EntityId);
        

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

    private List<string> GetFilesToDelete(ImageMetadata oldMetadata, ImageMetadata updatedMetadata)
    {
        var oldFileNames = oldMetadata.FileNames;
        var updatedFileNames = updatedMetadata.FileNames;
        
        var oldSet = new HashSet<string>(oldFileNames);
        var updatedSet = new HashSet<string>(updatedFileNames);
        
        // знаходимо пересічні файли, які залишаються
        var remain = oldSet.Intersect(updatedSet).ToList();
        
        // new files only in updated
        var added = updatedSet.Except(oldSet).ToList();
        
        // files to delete
        var toDelete = oldSet.Except(updatedSet).ToList();
        
        _logger.LogInformation("Remain: {count} files, Added: {count} files, ToDelete: {count} files",
            remain.Count, added.Count, toDelete.Count);
        
        return toDelete;
        
    }


}