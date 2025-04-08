using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Domain
{
    public class ImageService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ImageStorageSettings _settings;

        public ImageService(
            IHostEnvironment environment,
            ImageStorageSettings settings)
        {
            _hostEnvironment = environment;
            _settings = settings;
        }

        public async Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> imageFiles)
        {
            var urlsList = new List<string>();

            foreach (var imageFile in imageFiles)
            {
                string imageUrl = await UploadImageAsync(imageFile);
                urlsList.Add(imageUrl);
            }

            return urlsList;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            ValidateImage(imageFile);

            string fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

            string path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images");
            Directory.CreateDirectory(path);

            var compressedContent = await CompressImage(imageFile, fileExtension);

            var uniqueFileName = GenerateUniqueImageName(imageFile.FileName);
            var filePath = Path.Combine(path, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, compressedContent);

            var normalizedFilePath = filePath.Replace("\\", "/");

            return normalizedFilePath;
        }

        private void ValidateImage(IFormFile file)
        {
            if (file == null)
                throw new CustomException(CustomExceptionType.InvalidInputData, "The image field cannot be left empty.");

            if (file.Length == 0)
                throw new CustomException(CustomExceptionType.InvalidInputData, "Uploaded file is empty.");

            if (file.Length > _settings.MaxImageFileSizeInBytes)
                throw new CustomException(CustomExceptionType.InvalidInputData,
                    $"Image size must be less than {_settings.MaxImageFileSizeInBytes / 1024 / 1024} MB.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_settings.AllowedFileExtensions.Contains(extension))
                throw new CustomException(CustomExceptionType.InvalidInputData, "Unsupported file extension.");
        }

        private async Task<byte[]> CompressImage(IFormFile imageFile, string fileExtension)
        {
            using var imageStream = imageFile.OpenReadStream();
            using var image = await Image.LoadAsync(imageStream);

            if (TryParseSize(_settings.MaxProductFileSize, out var maxSize))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(maxSize.Width, maxSize.Height),
                    Mode = ResizeMode.Max
                }));
            }

            using var memoryStream = new MemoryStream();
            var encoder = GetEncoder(fileExtension);
            await image.SaveAsync(memoryStream, encoder);
            return memoryStream.ToArray();
        }

        private IImageEncoder GetEncoder(string extension)
        {
            var allowedJpegExtensions = _settings.AllowedFileExtensions.Where(ext => ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg").ToList();
            var allowedPngExtensions = _settings.AllowedFileExtensions.Where(ext => ext.ToLower() == ".png").ToList();
            var allowedWebpExtensions = _settings.AllowedFileExtensions.Where(ext => ext.ToLower() == ".webp").ToList();

            return extension.ToLower() switch
            {
                _ when allowedJpegExtensions.Contains(extension.ToLower()) => new JpegEncoder
                {
                    Quality = _settings.Jpeg.Quality
                },
                _ when allowedPngExtensions.Contains(extension.ToLower()) => new PngEncoder
                {
                    CompressionLevel = (PngCompressionLevel)_settings.Png.CompressionLevel
                },
                _ when allowedWebpExtensions.Contains(extension.ToLower()) => new WebpEncoder
                {
                    Quality = _settings.Webp.Quality,
                    FileFormat = Enum.TryParse<WebpFileFormatType>(_settings.Webp.FileFormat, out var fileFormat)
                        ? fileFormat
                        : WebpFileFormatType.Lossy
                },
                _ => throw new NotSupportedException($"No encoder available for extension {extension}")
            };
        }

        private string GenerateUniqueImageName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            return $"{Guid.NewGuid():N}{fileExtension}";
        }

        private bool TryParseSize(string? sizeString, out (int Width, int Height) size)
        {
            size = default;
            if (string.IsNullOrWhiteSpace(sizeString)) return false;

            var parts = sizeString.Split('*');
            if (parts.Length != 2) return false;

            if (int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
            {
                size = (width, height);
                return true;
            }

            return false;
        }
    }
}
