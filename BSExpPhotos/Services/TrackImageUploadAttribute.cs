using BSExpPhotos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSExpPhotos.Metadata;

namespace BSExpPhotos.Services
{
    public class TrackImageUploadAttribute : IAsyncActionFilter
    {
        private ImageMetadata _imageUploadMetadata;
        private readonly IImageInfoExtractor _imageInfoExtractor;
        private readonly ILogger<TrackImageUploadAttribute> _logger;

        public TrackImageUploadAttribute(IImageInfoExtractor imageInfoExtractor, 
            ImageMetadata imageUploadMetadata, ILogger<TrackImageUploadAttribute> logger)
        {
            _imageInfoExtractor = imageInfoExtractor;
            _imageUploadMetadata = imageUploadMetadata;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try { 
            var executedContext = await next();
                // Получаем возвращённый результат
                    if (executedContext.Result is ObjectResult objectResult && objectResult.Value != null)
                    {
                        var returnedEntity = objectResult.Value;
                        var entityType = returnedEntity.GetType().Name; // Например: "Product" или "Promotion"

                        _imageUploadMetadata.EntityType = entityType switch
                        {
                            "Product" => Photo.EntityType.Product,
                            "Promotion" => Photo.EntityType.Promotion,
                            "Category" => Photo.EntityType.Category,
                            _ => Photo.EntityType.None

                        };


                        var idProperty = returnedEntity.GetType().GetProperty("Id");
                        var entityId = idProperty?.GetValue(returnedEntity);

                        _imageUploadMetadata.EntityId = entityId switch
                        {
                            Guid g => g,
                            string s when Guid.TryParse(s, out var parsed) => parsed,
                            _ => Guid.Empty
                        };

                        _imageUploadMetadata.CreatedAt = DateTime.UtcNow;

                        _imageUploadMetadata.FileNames = _imageInfoExtractor.ExtractImageFileNames(returnedEntity);
                        if (_imageUploadMetadata.FileNames == null || !_imageUploadMetadata.FileNames.Any())
                        {
                            _logger.LogWarning("No image file names found for {EntityType} with ID {EntityId}",
                                _imageUploadMetadata.EntityType, _imageUploadMetadata.EntityId);
                            return;
                        }

                        if (_imageUploadMetadata.FileNames.Any())
                        { 
                            var service = context.HttpContext.RequestServices.GetRequiredService<IImageUploadMetadataService>();

                            foreach (string fileName in _imageUploadMetadata.FileNames)
                            {
                                await service.SaveImageInfoToDb(fileName, _imageUploadMetadata.EntityType, _imageUploadMetadata.EntityId, _imageUploadMetadata.CreatedAt);
                        
                                _logger.LogInformation("Image metadata saved for {EntityType} with ID {EntityId} with fileName {fileName}",
                                    _imageUploadMetadata.EntityType, _imageUploadMetadata.EntityId, fileName);
                            }

                            _logger.LogInformation("Finished processing image upload metadata for {EntityType} with ID {EntityId}",
                                _imageUploadMetadata.EntityType, _imageUploadMetadata.EntityId);
                        }
                    
                }

        }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while tracking image upload metadata");
            }

        }

    }


}