using Azure.Identity;
using BSExpPhotos.Interfaces;
using BSExpPhotos.Metadata;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSExpPhotos.Services
{
    public class ImageUploadMetadataService(OnlineStoreDbContext _dbContext) : IImageUploadMetadataService
    {
        public async Task SaveImageInfoToDb(string fileName, DAL.Photo.EntityType entityType, Guid entityId, DateTime uploadedAt)
        {
            var record = new Photo
            {
                FileName = fileName,
                EntityId = entityId,
                UploadedAt = uploadedAt,
                Type = entityType
            };

            _dbContext.Photos.Add(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}
