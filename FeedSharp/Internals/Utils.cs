using System.Globalization;

namespace FeedSharp.Internals;

internal static class Utils
{
    /// <summary>
    /// Default generator name
    /// </summary>
    public static string Generator =>  "https://github.com/tatwd/FeedSharp";
    
    /// <summary>
    /// Check and replace '&amp;' to '&amp;amp;'
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string? Sanitize(string? url) => url is null ? url : url.Replace("&", "&amp;");

    /// <summary>
    /// Format DateTime just like '2023-03-11T14:19:32.673Z'
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToIsoString(DateTime dateTime) =>
        dateTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
    
    /// <summary>
    /// Format DateTime just like 'Wed, 01 Nov 2023 12:11:00 GMT'
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToUtcString(DateTime dateTime) =>
        dateTime.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
}