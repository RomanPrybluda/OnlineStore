using BSExpPhotos.Interfaces;
using BSExpPhotos.Metadata;
using BSExpPhotos.Services;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Middleware
{
    public class ImageCleanupMiddlewareForDeleteMethods
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ImageCleanupMiddlewareForDeleteMethods> _logger;
        private readonly ImageInfoExtractor _imageInfoExtractor;
        private readonly IImageUploadMetadataService _imageUploadMetadataService;
        

        public ImageCleanupMiddlewareForDeleteMethods(RequestDelegate next, ILogger<ImageCleanupMiddlewareForDeleteMethods> logger, 
            ImageInfoExtractor imageInfoExtractor, IImageUploadMetadataService imageUploadMetadataService)
        {
            _next = next;
            _logger = logger;
            _imageInfoExtractor = imageInfoExtractor;
            _imageUploadMetadataService = imageUploadMetadataService;
        }

        public async Task InvokeAsync(HttpContext context, OnlineStoreDbContext dbContext)
        {
            var isDelete = context.Request.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
            var routeData = context.GetRouteData();

            string? controller = routeData.Values["controller"]?.ToString();
            string? id = routeData.Values["id"]?.ToString();
            
            var entityId = Guid.TryParse(id, out var parsed) ? parsed : Guid.Empty;

            if (entityId == Guid.Empty)
            {
                _logger.LogWarning("No entity found with id {id}", entityId);
                return;
            }
            
            await _next(context);
            
            if (isDelete && context.Response.StatusCode == StatusCodes.Status204NoContent)
            {
                await HandleDeleteAsync(controller, entityId);
            }
        }

        private async Task HandleDeleteAsync(string? controller, Guid id)
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
                return;
            }


            try
            {
                var metadata = new ImageMetadata
                {
                    EntityType = entityType,
                    CreatedAt = DateTime.UtcNow,
                    FileNames =  await _imageInfoExtractor.ExtractImageFileNames(id, entityType),
                    EntityId = id
                };

                if (metadata.FileNames.Count == 0)
                {
                    _logger.LogWarning("No photos found for {controller}/{id}", controller, id);
                    return;
                }
                    
                // save info to db
                foreach (var fileName in metadata.FileNames)
                    await _imageUploadMetadataService.SaveImageInfoToDb(fileName, metadata.EntityType,
                        metadata.EntityId, metadata.CreatedAt);
                
                _logger.LogInformation("Marked photos as deleted for entity {id} in {Controller}", id, controller);

            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                _logger.LogError(ex, "Error while marking as deleted info about photos for entity {id} in controller {Controller}", 
                    id, controller);
                throw;
            }
        }

       
       
    }
    
    
}
