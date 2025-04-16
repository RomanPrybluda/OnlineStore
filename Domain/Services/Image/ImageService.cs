using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Domain
{
    public class ImageService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ImageStorageSettings _settings;

        public ImageService(IHostEnvironment environment, ImageStorageSettings settings)
        {
            _hostEnvironment = environment;
            _settings = settings;
        }

        public async Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> imageFiles)
        {
            var baseFileNames = new List<string>();

            foreach (var imageFile in imageFiles)
            {
                string baseFileName = await UploadImageAsync(imageFile);
                baseFileNames.Add(baseFileName);
            }

            return baseFileNames;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            ValidateImage(imageFile);

            string outputDirectory = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images");
            Directory.CreateDirectory(outputDirectory);

            string baseFileName = GenerateUniqueImageNameWithoutExtension();

            using var image = await Image.LoadAsync(imageFile.OpenReadStream());

            var variants = _settings.ImageVariants;

            foreach (var (suffix, width) in variants)
            {
                var clone = image.Clone(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, 0)
                }));

                string fullFileName = $"{baseFileName}-{suffix}{_settings.SavedFileExtension}";
                string fullPath = Path.Combine(outputDirectory, fullFileName);

                await using var stream = new FileStream(fullPath, FileMode.Create);
                await clone.SaveAsync(stream, new WebpEncoder
                {
                    Quality = _settings.Webp.Quality,
                    FileFormat = Enum.TryParse<WebpFileFormatType>(_settings.Webp.FileFormat, out var format)
                        ? format
                        : WebpFileFormatType.Lossy
                });
            }

            return baseFileName;
        }

        private void ValidateImage(IFormFile file)
        {
            if (file == null)
                throw new CustomException(CustomExceptionType.InvalidInputData, "The image field cannot be left empty.");

            if (file.Length == 0)
                throw new CustomException(CustomExceptionType.InvalidInputData, "Uploaded file is empty.");

            if (file.Length < _settings.MinImageFileSizeInBytes)
                throw new CustomException(CustomExceptionType.InvalidInputData,
                    $"Image size must be at least {_settings.MinImageFileSizeInBytes / 1024} KB.");

            if (file.Length > _settings.MaxImageFileSizeInBytes)
                throw new CustomException(CustomExceptionType.InvalidInputData,
                    $"Image size must be less than {_settings.MaxImageFileSizeInBytes / 1024 / 1024} MB.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_settings.AllowedFileExtensions.Contains(extension))
                throw new CustomException(CustomExceptionType.InvalidInputData, "Unsupported file extension.");
        }

        private string GenerateUniqueImageNameWithoutExtension()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");
            string guid = Guid.NewGuid().ToString("N");
            return $"{timestamp}_{guid}";
        }
    }
}
