namespace Uniring.Application.Utils
{
    public static class MediaTypeHandler
    {
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        private static readonly HashSet<string> AllowedVideoExtensions = new(StringComparer.OrdinalIgnoreCase) { ".mp4", ".webm", ".mov", ".mkv" };

        public static bool isValidVideo(string ext)
        {
            return AllowedVideoExtensions.Contains(ext);
        }

        public static bool isValidImage(string ext)
        {
            return AllowedImageExtensions.Contains(ext);
        }

        public static bool isValidMedia(string ext)
        {
            return isValidImage(ext) || isValidVideo(ext);
        }
    }
}
