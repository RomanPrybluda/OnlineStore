using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BSExpPhotos.Services;

public class PhotoCleanupService
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILogger<PhotoCleanupService> _logger;
    private readonly string _photoDirectory;

    public PhotoCleanupService(
        OnlineStoreDbContext dbContext,
        ILogger<PhotoCleanupService> logger,
        IHostEnvironment hostEnvironment) // Add this parameter
    {
        _dbContext = dbContext;
        _logger = logger;

        // Use WebRootPath from IWebHostEnvironment to get the wwwroot directory
        _photoDirectory = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot/photos");

        // Alternative: If you need content root instead of wwwroot
        // _photoDirectory = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot/photos");
    }

    public async Task CleanUpOutdatedPhotosAsync()
    {
        try
        {
            _logger.LogInformation("Starting photo cleanup process at {Time}", DateTime.UtcNow);

            // Get list of filenames from database
            var validFileNames = await _dbContext.Photos
                .Where(p => p.IsDeleted == false)
                .Select(p => p.FileName)
                .ToListAsync();

            if (!Directory.Exists(_photoDirectory))
            {
                _logger.LogWarning("Photo directory does not exist: {Directory}", _photoDirectory);
                return;
            }

            // Get list of files on server
            var serverFiles = Directory.GetFiles(_photoDirectory)
                .Select(Path.GetFileName)
                .ToList();

            // Identify outdated files (files on server but not in database)
            var outdatedFiles = serverFiles
                .Where(file => file != null && validFileNames.Contains(file) == false)
                .ToList();

            if (outdatedFiles.Count == 0)
            {
                _logger.LogWarning("No outdated files found: {File}", _photoDirectory);
                return;
            }


            // Delete outdated files
            foreach (var file in outdatedFiles)
                try
                {
                    if (file == null)
                        continue;

                    var filePath = Path.Combine(_photoDirectory, file);
                    File.Delete(filePath);
                    _logger.LogInformation("Deleted outdated file: {File}", file);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete file: {File}", file);
                }

            await DeleteMarkedPhotosAsync();

            _logger.LogInformation("Photo cleanup completed. Deleted {Count} files.", outdatedFiles.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during photo cleanup process.");
        }
    }

    private async Task DeleteMarkedPhotosAsync()
    {
        var deletedFiles = await _dbContext.Photos
            .Where(p => p.IsDeleted == true)
            .ToListAsync();
        _dbContext.RemoveRange(deletedFiles);

        await _dbContext.SaveChangesAsync();
    }
}