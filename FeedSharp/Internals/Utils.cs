using System.Globalization;
using System.Xml;

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
    
    public static async Task WriteElementAsync(XmlWriter writer, string name, string? val)
    {
        if (val is null)
            return;

        await writer.WriteStartElementAsync("", name, null);  
        await writer.WriteStringAsync(val);  
        await writer.WriteEndElementAsync();
    }

    public static Task WriteHtmlCdataElementAsync(XmlWriter writer, string name, string? html) =>
        WriteHtmlCdataElementAsync(writer, name, html, true);

    public static Task WriteCdataElementAsync(XmlWriter writer, string name, string? cdata) =>
        WriteHtmlCdataElementAsync(writer, name, cdata, false);

    private static async Task WriteHtmlCdataElementAsync(XmlWriter writer, string name, string? cdata, bool isHtml)
    {
        if (cdata is null)
            return;

        var nameWithPrefix = name.Split(':');
        if (nameWithPrefix.Length == 2)
        {
            await writer.WriteStartElementAsync(nameWithPrefix[0], nameWithPrefix[1], null);
        }
        else
        {
            await writer.WriteStartElementAsync("", name, null);
        }
        
        if (isHtml)
            await writer.WriteAttributeStringAsync(null, "type", null, "html");  
        
        await writer.WriteCDataAsync(cdata);  
        await writer.WriteEndElementAsync();
    }
    
    public static async Task WriteCategoryElementAsync(XmlWriter writer, Category category)
    {
        await writer.WriteStartElementAsync("", "category", null);  
        
        if (category.Name != null)
            await writer.WriteAttributeStringAsync(null, "label", null, category.Name);  
        
        if (category.Scheme != null)
            await writer.WriteAttributeStringAsync(null, "scheme", null, category.Scheme);  

        if (category.Term != null)
            await writer.WriteAttributeStringAsync(null, "term", null, category.Term);
        
        await writer.WriteEndElementAsync();
    }
}