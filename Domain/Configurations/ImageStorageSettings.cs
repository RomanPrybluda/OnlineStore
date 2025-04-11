namespace Domain
{
    public class ImageStorageSettings
    {
        public string ContentType { get; set; }

        public string MaxAvatarImageFileSize { get; set; }

        public string MaxProductFileSize { get; set; }

        public int MaxImageFileSizeInBytes { get; set; }

        public List<string> AllowedFileExtensions { get; set; }

        public JpegSettings Jpeg { get; set; }

        public PngSettings Png { get; set; }

        public WebpSettings Webp { get; set; }

    }

    public class JpegSettings
    {
        public int Quality { get; set; }
    }

    public class PngSettings
    {
        public int CompressionLevel { get; set; }
    }

    public class WebpSettings
    {
        public int Quality { get; set; }

        public string FileFormat { get; set; }
    }
}
