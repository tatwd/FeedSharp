using System.Text;
using System.Xml;

namespace FeedSharp.Internals;

public class Rss2
{
    private readonly Feed _feed;

    public Rss2(Feed feed) => _feed = feed;

    public string Render() => RenderAsync().GetAwaiter().GetResult();
    
    public async Task<string> RenderAsync()
    {
        var options = _feed.Options;
        
        var settings = new XmlWriterSettings
        {
            Async = true,
            Encoding = Encoding.UTF8,
            Indent = true
        };
        var outSb = new Utf8StringWriter();
        using var writer = XmlWriter.Create(outSb, settings);

        // <rss>
        await writer.WriteStartElementAsync("", "rss", null);
        await writer.WriteAttributeStringAsync(null, "version", null, "2.0");

        var isContent = _feed.Items.Any(x => x.Content != null);
        if (isContent)
        {
            await writer.WriteAttributeStringAsync("xmlns", "dc", null, "http://purl.org/dc/elements/1.1/");
            await writer.WriteAttributeStringAsync("xmlns", "content", null, "http://purl.org/rss/1.0/modules/content/");
        }
        
        var hubLink = options.Hub;
        var atomLink = options.Feed ?? options.FeedLinks?.Rss;
        var isAtom = hubLink != null || atomLink != null;
        if (isAtom)
        {
            await writer.WriteAttributeStringAsync("xmlns", "atom", null, "http://www.w3.org/2005/Atom");
        }

        // <channel>
        await writer.WriteStartElementAsync("", "channel", null);

        var elements = new List<(string name, string? val)>
        {
            ("title", options.Title),
            ("link", Utils.Sanitize(options.Link)),
            ("description", Utils.Sanitize(options.Description)),
            ("lastBuildDate", Utils.ToUtcString(options.Updated ?? DateTime.Now)),
            ("docs", options.Docs ?? "https://validator.w3.org/feed/docs/rss2.html"),
            ("generator", Utils.Sanitize(options.Generator ?? Utils.Generator)),
        };

        foreach (var el in elements)
        {
            await Utils.WriteElementAsync(writer, el.name, el.val);
        }
        
        await Utils.WriteElementAsync(writer, "language", options.Language);
        await Utils.WriteElementAsync(writer, "ttl", options.Ttl?.ToString() ?? null);

        if (options.Image != null)
        {
            await writer.WriteStartElementAsync("", "image", null);
            await Utils.WriteElementAsync(writer, "title", options.Title);
            await Utils.WriteElementAsync(writer, "url", options.Image);
            await Utils.WriteElementAsync(writer, "link", options.Link);
            await writer.WriteEndElementAsync();  
        }

        await Utils.WriteElementAsync(writer, "copyright", options.Copyright);

        foreach (var category in _feed.Categories)
        {
            await Utils.WriteElementAsync(writer, "category", category);
        }

        if (hubLink != null)
        {
            await writer.WriteStartElementAsync("atom", "link", null);
            await writer.WriteAttributeStringAsync(null, "href", null, Utils.Sanitize(hubLink)!);
            await writer.WriteAttributeStringAsync(null, "rel", null, "hub");
            await writer.WriteEndElementAsync();  
        }
        else
        {
            if (atomLink != null)
            {
                await writer.WriteStartElementAsync("atom", "link", null);
                await writer.WriteAttributeStringAsync(null, "href", null, Utils.Sanitize(atomLink)!);
                await writer.WriteAttributeStringAsync(null, "rel", null, "sef");
                await writer.WriteAttributeStringAsync(null, "type", null, "application/rss+xml");
                await writer.WriteEndElementAsync();  
            }
        }

        foreach (var item in _feed.Items)
        {
            await WriteEntryElementAsync(writer, item);
        }

        // </channel>
        await writer.WriteEndElementAsync();  
        
        // </rss>
        await writer.WriteEndElementAsync();  
        await writer.FlushAsync();  
        
        return outSb.ToString();
    }
    
    private static async Task WriteEntryElementAsync(XmlWriter writer, Item item)
    {
        await writer.WriteStartElementAsync("", "item", null);

        await Utils.WriteCdataElementAsync(writer, "title", item.Title);
        await Utils.WriteElementAsync(writer, "link", item.Link);

        var guid = item.Guid ?? item.Id ?? Utils.Sanitize(item.Link);
        await Utils.WriteElementAsync(writer, "guid", guid);

        await Utils.WriteElementAsync(writer, "pubDate", Utils.ToUtcString(item.Published ?? item.Date));
        await Utils.WriteCdataElementAsync(writer, "description", item.Description); 
        await Utils.WriteCdataElementAsync(writer, "content:encoded", item.Content);  
        
        foreach (var author in item.Author)
        {
            if (author.Email is null || author.Name is null)
            {
                continue;
            }
            await Utils.WriteElementAsync(writer, "author", $"{author.Email}({author.Name})");
        }

        foreach (var category in item.Category)
        {
            await WriteCategoryElementAsync(writer, category);
        }

        await WriteEnclosureElementAsync(writer, item.Enclosure);
        await WriteEnclosureElementAsync(writer, item.Image);
        await WriteEnclosureElementAsync(writer, item.Audio, "audio");
        await WriteEnclosureElementAsync(writer, item.Video, "video");

        await writer.WriteEndElementAsync();  
    }

    private static async Task WriteCategoryElementAsync(XmlWriter writer, Category category)
    {
        await writer.WriteStartElementAsync("", "category", null);  
        
        if (category.Domain != null)
            await writer.WriteAttributeStringAsync(null, "domain", null, category.Domain);  
        
        await writer.WriteStringAsync(category.Name);
        await writer.WriteEndElementAsync();
    }

    private static async Task WriteEnclosureElementAsync(XmlWriter writer, Enclosure? enclosure, string mimeCategory = "image")
    {
        if (enclosure is null)
            return;

        await writer.WriteStartElementAsync("", "enclosure", null);

        if (enclosure.Length.HasValue)
            await writer.WriteAttributeStringAsync(null, "length", null, enclosure.Length.Value.ToString());
        
        var type = enclosure.Url.Split('.').LastOrDefault() ?? enclosure.Type;
        await writer.WriteAttributeStringAsync(null, "type", null, $"{mimeCategory}/{type}");
        await writer.WriteAttributeStringAsync(null, "url", null, enclosure.Url);
        await writer.WriteAttributeStringAsync(null, "title", null, enclosure.Title);
        
        if (enclosure.Duration.HasValue)
            await writer.WriteAttributeStringAsync(null, "duration", null, enclosure.Duration.Value.ToString());

        await writer.WriteEndElementAsync();  
    }
    
}