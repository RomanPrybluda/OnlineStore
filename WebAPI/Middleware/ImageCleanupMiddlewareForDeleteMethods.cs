using BSExpPhotos.Interfaces;
using BSExpPhotos.Metadata;
using BSExpPhotos.Services;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Middleware
{
    public class ImageCleanupMiddlewareForDeleteMethods: IMiddleware
    {
        private readonly ILogger<ImageCleanupMiddlewareForDeleteMethods> _logger;
        private readonly IImageInfoExtractor _imageInfoExtractor;
        private readonly IImageUploadMetadataService _imageUploadMetadataService;
        

        public ImageCleanupMiddlewareForDeleteMethods( ILogger<ImageCleanupMiddlewareForDeleteMethods> logger, 
            IImageInfoExtractor imageInfoExtractor, IImageUploadMetadataService imageUploadMetadataService)
        {
            _logger = logger;
            _imageInfoExtractor = imageInfoExtractor;
            _imageUploadMetadataService = imageUploadMetadataService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var isDelete = context.Request.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
            
            if (!isDelete)
            {
                await next(context);
                return;
            }
            
            var routeData = context.GetRouteData();

            string? controller = routeData.Values["controller"]?.ToString();
            string? id = routeData.Values["id"]?.ToString();
            
            var entityId = Guid.TryParse(id, out var parsed) ? parsed : Guid.Empty;

            if (entityId == Guid.Empty)
            {
                _logger.LogWarning("No entity found with id {id}", entityId);
                await next(context);
                return;
            }

          
            var metadata = await HandleDeleteAsync(controller, entityId);
            
            
            await next(context);

            if (context.Response.StatusCode == StatusCodes.Status204NoContent)
            {
                // save info to db
                foreach (var fileName in metadata.FileNames)
                    await _imageUploadMetadataService.SaveImageInfoToDb(fileName, metadata.EntityType,
                        metadata.EntityId, metadata.CreatedAt);

                _logger.LogInformation("Marked photos as deleted for entity {id} in {Controller}", id, controller);
            }

        }

        private async Task<ImageMetadata> HandleDeleteAsync(string? controller, Guid id)
        {

            var entityType = controller?.ToLower() switch
            {
                "product" => Photo.EntityType.Product,
                "promotions" => Photo.EntityType.Promotion,
                "category" => Photo.EntityType.Category,
                _ => Photo.EntityType.None
            };
            _logger.LogInformation("entity type for {controller}/{id}. Entity type: {entityType}",
                controller, id, entityType);

            if (entityType == Photo.EntityType.None)
            {
                _logger.LogWarning("No entity type found for {controller}/{id}", controller, id);
                return  new ImageMetadata();
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
                    
                }
                
                return metadata;
               
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
