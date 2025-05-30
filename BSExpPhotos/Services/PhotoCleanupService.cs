using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Collections.Generic;
using BSExpPhotos.Interfaces;


namespace BSExpPhotos.Services;

public class PhotoCleanupService: IImageCleanupService
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILogger<PhotoCleanupService> _logger;
    private readonly string _photoDirectory;

    public PhotoCleanupService(
        OnlineStoreDbContext dbContext,
        ILogger<PhotoCleanupService> logger,
        IHostEnvironment hostEnvironment) 
    {
        _dbContext = dbContext;
        _logger = logger;

        // Use WebRootPath from IWebHostEnvironment to get the wwwroot directory
        _photoDirectory = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot/images");
    }

    public async Task CleanUpOutdatedPhotosAsync()
    {
        try
        {
            if (!Directory.Exists(_photoDirectory))
            {
                _logger.LogWarning("Photo directory does not exist: {Directory}", _photoDirectory);
                return;
            }
            
            _logger.LogInformation("Starting photo cleanup process at {Time}", DateTime.UtcNow);

            // Получить список всех допустимых имен файлов с суффиксами
            var baseNamesForDelete = await _dbContext.Photos
                .Where(p => p.IsDeleted)
                .Select(p => Path.GetFileNameWithoutExtension(p.FileName)) // убираем расширение
                .ToListAsync();

            var validFileNameSet = baseNamesForDelete
                .SelectMany(baseName => new[]
                {
                    $"{baseName}-desktop.webp",
                    $"{baseName}-mobile.webp",
                    $"{baseName}-tablet.webp"
                })
                .ToHashSet(StringComparer.OrdinalIgnoreCase); // для ускорения поиска и без учета регистра

          
            if (validFileNameSet.Count == 0)
            {
                _logger.LogWarning("No outdated files found in directory: {Directory}", _photoDirectory);
                await RemoveDeletedRecordsAsync();
                _logger.LogInformation("Photo cleanup completed. Deleted records in db, but without outdated files.");
                return;
            }


            // Delete outdated files
            foreach (var file in validFileNameSet)
                try
                {
                    if (file.Length == 0)
                        continue;

                    var filePath = Path.Combine(_photoDirectory, file);
                    if (File.Exists(filePath))
                    {
                        File.SetAttributes(filePath, FileAttributes.Normal);
                        File.Delete(filePath);
                        _logger.LogInformation("Deleted outdated file: {File}", file);
                    }
                    else
                    {
                        _logger.LogInformation("Outdated file not exist: {File}", file);
                    }
                    
                   
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete file: {File}", file);
                }

            await RemoveDeletedRecordsAsync();

            _logger.LogInformation("Photo cleanup completed. Deleted {Count} files.", validFileNameSet.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during photo cleanup process.");
        }
    }
    
    public async Task MarkRemovedImagesAsDeletedAsync(
        Guid entityId,
        Photo.EntityType entityType,
        List<string> newFileNames,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var existingFileNames = await _dbContext.Photos
                .Where(p => p.EntityId == entityId &&
                            p.Type == entityType &&
                            !p.IsDeleted)
                .Select(p => p.FileName)
                .ToListAsync(cancellationToken);

            var removedFiles = existingFileNames.Except(newFileNames).ToList();

            if (removedFiles.Count == 0)
                return;

            var photosToMarkDeleted = await _dbContext.Photos
                .Where(p => removedFiles.Contains(p.FileName) &&
                            p.EntityId == entityId &&
                            p.Type == entityType)
                .ToListAsync(cancellationToken);

            foreach (var photo in photosToMarkDeleted)
                photo.IsDeleted = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Marked the following images as deleted: {Files}", string.Join(", ", removedFiles));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while marking removed images as deleted for EntityId: {EntityId}", entityId);
            throw;
        }
    }
    
    public async Task RemoveDeletedRecordsAsync()
    {
        var deletedFiles = await _dbContext.Photos
            .Where(p => p.IsDeleted == true)
            .ToListAsync();
        _dbContext.RemoveRange(deletedFiles);

        await _dbContext.SaveChangesAsync();
    }
    
}