using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BSExpPhotos.Services
{
    /// <summary>
    /// Service to clean up outdated photos from the server.
    /// </summary>
    /// <remarks>
    /// This service checks for photos on the server that are not referenced in the database and deletes them.
    /// </remarks>
    public class PhotoCleanupService
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILogger<PhotoCleanupService> _logger;
    private readonly string _photoDirectory;

    public PhotoCleanupService(OnlineStoreDbContext dbContext, ILogger<PhotoCleanupService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _photoDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos"); // Adjust path as needed
    }

    public async Task CleanUpOutdatedPhotosAsync()
    {
        try
        {
            _logger.LogInformation("Starting photo cleanup process at {Time}", DateTime.UtcNow);

            // Get list of filenames from database
            var validFileNames = await _dbContext.Photos
                .Select(p => p.FileName)
                .ToListAsync();

            // Get list of files on server
            var serverFiles = Directory.Exists(_photoDirectory)
                ? [.. Directory.GetFiles(_photoDirectory).Select(Path.GetFileName)]
                : new List<string>();

            // Identify outdated files (files on server but not in database)
            var outdatedFiles = serverFiles
                .Where(file => !validFileNames.Contains(file))
                .ToList();

            // Delete outdated files
            foreach (var file in outdatedFiles)
            {
                try
                {
                    var filePath = Path.Combine(_photoDirectory, file);
                    File.Delete(filePath);
                    _logger.LogInformation("Deleted outdated file: {File}", file);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete file: {File}", file);
                }
            }

            _logger.LogInformation("Photo cleanup completed. Deleted {Count} files.", outdatedFiles.Count);
        }
        catch (Exception ex)
        {   
            _logger.LogError(ex, "Error during photo cleanup process.");
        }
    }
}
}