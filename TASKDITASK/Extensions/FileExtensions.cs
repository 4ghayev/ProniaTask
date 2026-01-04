namespace TASKDITASK.Extensions
{
    public static class FileExtensions
    {
        public static bool IsImage(this IFormFile file)
            => file.ContentType.Contains("image");

        public static bool IsAllowedSize(this IFormFile file, int mb)
            => file.Length / 1024 / 1024 <= mb;
    }
}
