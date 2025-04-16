namespace Domain
{
    public class ImageStorageSettings
    {

        public string MaxAvatarImageFileSize { get; set; }

        public string MaxProductFileSize { get; set; }

        public int MaxImageFileSizeInBytes { get; set; }

        public string MinProductFileSize { get; set; }

        public int MinImageFileSizeInBytes { get; set; }

        public List<string> AllowedFileExtensions { get; set; }

        public WebpSettings Webp { get; set; }

        public Dictionary<string, int> ImageVariants { get; set; }

        public string SavedFileExtension { get; set; }
    }


    public class WebpSettings
    {
        public int Quality { get; set; }

        public string FileFormat { get; set; }
    }
}
