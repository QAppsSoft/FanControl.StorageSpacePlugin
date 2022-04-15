namespace FanControl.StorageSpacePlugin.Infrastructure.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNullEmptyOrWhite(this string sValue)
        {
            return string.IsNullOrWhiteSpace(sValue);
        }
    }
}
