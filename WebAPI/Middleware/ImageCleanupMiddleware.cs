using DAL;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Middleware
{
    public class ImageCleanupMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ImageCleanupMiddleware> _logger;

        public ImageCleanupMiddleware(RequestDelegate next, ILogger<ImageCleanupMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, OnlineStoreDbContext dbContext)
        {
            var isDelete = context.Request.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
            var isUpdate = context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase);
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
            
            if (isDelete)
            {
                await HandleDeleteAsync(controller, entityId, dbContext);
            }
            else if (isUpdate)
            {
                await HandleUpdateAsync(controller, entityId, dbContext);
            }
            
            
           

        }

        private async Task HandleDeleteAsync(string? controller, Guid id, OnlineStoreDbContext dbContext)
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
                var photos = dbContext.Photos
                   .Where(p => p.EntityId == id);

                if (!photos.Any())
                {
                    _logger.LogWarning("No photos found for {controller}/{id}", controller, id);
                    return;
                }
                    

                foreach (var photo in photos)
                    photo.IsDeleted = true;

                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Marked photos as deleted for entity {id} in {Controller}", id, controller);

            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                _logger.LogError(ex, "Error while deleting info about photos from db for entity {id} in controller {Controller}", 
                    id, controller);
                throw;
            }
        }

        private async Task HandleUpdateAsync(string? controller, Guid id, OnlineStoreDbContext dbContext)
        {
            
        }
    }
    
    
}
