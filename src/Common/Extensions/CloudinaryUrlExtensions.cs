namespace Common.Extensions
{
    public static class CloudinaryUrlExtensions
    {
        public const string OptimizationParams = "f_auto,q_auto";
        public const string CloudinaryDefaultUrl = "https://res.cloudinary.com/";
        public const string UploadUrlParam = "upload";

        public static string TryGetOptimizedImageUrl(this string url)
        {
            if (!url.StartsWith(CloudinaryDefaultUrl))
            {
                return url;
            }
            var indexOfImageSpecs = url.IndexOf(UploadUrlParam) + UploadUrlParam.Length;
            var imageSpecs = url[indexOfImageSpecs..];
            var reqestPath = url[..indexOfImageSpecs];
            return $"{reqestPath}/{OptimizationParams}{imageSpecs}";
        }
    }
}
