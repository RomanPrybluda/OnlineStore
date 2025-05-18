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
            var routeData = context.GetRouteData();

            string? controller = routeData.Values["controller"]?.ToString();
            string? id = routeData.Values["id"]?.ToString();

            await _next(context);

            if (!isDelete || context.Response.StatusCode != StatusCodes.Status204NoContent || controller == null ||
                id == null)
            {
                _logger.LogInformation("""
                                   !isDelete || context.Response.StatusCode != StatusCodes.Status204NoContent || controller == null ||
                                   id == null
                                   {isDelete} || {context.Response.StatusCode} != StatusCodes.Status204NoContent || {controller} == null ||
                                   {id} == null
                                   """, isDelete, context.Response.StatusCode, controller, id);
                return;
            }
                

            var entityId = Guid.TryParse(id, out var parsed) ? parsed : Guid.Empty;

            if (entityId == Guid.Empty)
            {
                _logger.LogWarning("No entity found with id {id}", entityId);
                return;
            }
                


            var entityType = controller.ToLower() switch
            {
                "product" => Photo.EntityType.Product,
                "promotions" => Photo.EntityType.Promotion,
                "category" => Photo.EntityType.Category,
                _ => Photo.EntityType.None
            };
            _logger.LogInformation("entity type for {controller}/{entityId}", controller, entityId);

            if (entityType == Photo.EntityType.None)
            {
                _logger.LogWarning("No entity type found for {controller}/{entityId}", controller, entityId);
                return;
            }


            try
            {
                var photos = dbContext.Photos
                   .Where(p => p.EntityId == entityId);

                if (!photos.Any())
                {
                    _logger.LogWarning("No photos found for {controller}/{id}", controller, id);
                    return;
                }
                    

                foreach (var photo in photos)
                    photo.IsDeleted = true;

                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Marked photos as deleted for entity {EntityId} in {Controller}", entityId, controller);

            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                _logger.LogError(ex, "Error while deleting info about photos from db for entity {EntityId} in controller {Controller}", entityId, controller);
                throw;
            }

        }
    }
}
