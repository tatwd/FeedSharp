using System.Text;
using System.Xml;

namespace FeedSharp.Internals;

internal class Atom1
{
    private readonly Feed _feed;
    
    public Atom1(Feed feed) => _feed = feed;

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

        // <feed>
        await writer.WriteStartElementAsync("", "feed", "http://www.w3.org/2005/Atom");

        var elements = new List<(string name, string val)>
        {
            ("id", options.Id),
            ("title", options.Title),
            ("updated", Utils.ToIsoString(options.Updated ?? DateTime.Now)),
            ("generator", Utils.Sanitize(options.Generator ?? Utils.Generator))!,
        };

        foreach (var el in elements)
        {
            await Utils.WriteElementAsync(writer, el.name, el.val);
        }

        await WriteAuthorElementAsync(writer, options.Author);

        var alternateLink = Utils.Sanitize(options.Link);
        await WriteLinkElementAsync(writer, new Link { Rel = "alternate", Href = alternateLink  });

        var atomLink = Utils.Sanitize(options.Feed ?? options.FeedLinks?.Atom);
        await WriteLinkElementAsync(writer, new Link{ Rel = "self", Href = atomLink });
        
        var hubLink = Utils.Sanitize(options.Hub);
        await WriteLinkElementAsync(writer, new Link{ Rel = "hub", Href = hubLink });

        
        await Utils.WriteElementAsync(writer, "subtitle", options.Description);
        await Utils.WriteElementAsync(writer, "logo", options.Image);
        await Utils.WriteElementAsync(writer, "icon", options.Favicon);
        await Utils.WriteElementAsync(writer, "rights", options.Copyright);

        foreach (var category in _feed.Categories)
        {
            await WriteCategoryElementAsync(writer, new Category { Term = category });
        }

        foreach (var contributor in _feed.Contributors)
        {
            await WriteAuthorElementAsync(writer, contributor);
        }

        foreach (var item in _feed.Items)
        {
            await WriteEntryElementAsync(writer, item);
        }
        
        // </feed>
        await writer.WriteEndElementAsync();  
        await writer.FlushAsync();  
        
        return outSb.ToString();
    }

    private static async Task WriteEntryElementAsync(XmlWriter writer, Item item)
    {
        await writer.WriteStartElementAsync("", "entry", null);

        await Utils.WriteHtmlCdataElementAsync(writer, "title", item.Title);
        await Utils.WriteElementAsync(writer, "id", Utils.Sanitize(item.Id ?? item.Link));
        await WriteLinkElementAsync(writer, new Link { Href = item.Link });
        await Utils.WriteElementAsync(writer, "updated", Utils.ToIsoString(item.Date));

        await Utils.WriteHtmlCdataElementAsync(writer, "summary", item.Description);
        await Utils.WriteHtmlCdataElementAsync(writer, "content", item.Content);

        foreach (var category in item.Category)
        {
            await WriteCategoryElementAsync(writer, category);
        }
        
        foreach (var author in item.Author)
        {
            await WriteAuthorElementAsync(writer, author);
        }

        if (item.Published.HasValue)
            await Utils.WriteElementAsync(writer, "published", Utils.ToIsoString(item.Published.Value));
        
        await Utils.WriteElementAsync(writer, "rights", item.Copyright);
        
        await writer.WriteEndElementAsync();  
    }
    
    private static async Task WriteCategoryElementAsync(XmlWriter writer, Category category)
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

    private static async Task WriteAuthorElementAsync(XmlWriter writer, Author? author)
    {
        if (author is null)
            return;
        
        await writer.WriteStartElementAsync("", "author", null);
        await Utils.WriteElementAsync(writer, "name", author.Name);
        await Utils.WriteElementAsync(writer, "email", author.Email);
        await Utils.WriteElementAsync(writer, "link", author.Link);
        await writer.WriteEndElementAsync();
    }
    
    private static async Task WriteLinkElementAsync(XmlWriter writer, Link link)
    {
        if (link.Href is null)
            return;

        await writer.WriteStartElementAsync("", "link", null);
        
        if (link.Rel != null)
            await writer.WriteAttributeStringAsync(null, "rel", null, link.Rel);  
        
        await writer.WriteAttributeStringAsync(null, "href", null, link.Href);  
        await writer.WriteEndElementAsync();
    }
    
    private class Link
    {
        public string? Rel { get; set; }   
        public string? Href { get; set; }   
    }
    
}