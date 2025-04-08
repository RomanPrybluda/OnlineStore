namespace Domain
{
    public class ImageStorageSettings
    {
        public string ContentType { get; set; }

        public string MaxAvatarImageFileSize { get; set; }

        public string MaxProductFileSize { get; set; }

        public int MaxImageFileSizeInBytes { get; set; }

        public List<string> AllowedFileExtensions { get; set; }

        public int JpegQuality { get; set; }

        public int PngCompressionLevel { get; set; }

        public int WebpQuality { get; set; }

    }
}
