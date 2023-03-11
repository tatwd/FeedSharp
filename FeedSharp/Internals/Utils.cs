using System.Globalization;

namespace FeedSharp.Internals;

internal static class Utils
{
    public static string Generator =>  "https://github.com/tatwd/FeedSharp";
    
    public static string? Sanitize(string? url)
    {
        if (url is null)
        {
            return url;
        }
        return url.Replace("&", "&amp;");
    }

    public static string ToIsoString(DateTime dateTime) =>
        dateTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
}