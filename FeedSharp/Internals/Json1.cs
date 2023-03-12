using System.Text.Json;
using System.Text.Json.Serialization;

namespace FeedSharp.Internals;

internal class Json1
{
    private readonly Feed _feed;

    public Json1(Feed feed) => _feed = feed;

    public string Render() => RenderAsync().GetAwaiter().GetResult();

    public async Task<string> RenderAsync()
    {
        var options = _feed.Options;
        var items = _feed.Items;
        var extensions = _feed.Extensions;
        
        var feed = new Dictionary<string, object?>(16);

        AddIfNotNull(feed, "version", "https://jsonfeed.org/version/1");
        AddIfNotNull(feed, "title", options.Title);
        AddIfNotNull(feed, "home_page_url", options.Link);
        AddIfNotNull(feed, "feed_url", options.FeedLinks?.Json);
        AddIfNotNull(feed, "description", options.Description);
        AddIfNotNull(feed, "icon", options.Image);
        
        foreach (var extension in extensions)
        {
            AddIfNotNull(feed, extension.Name, extension.Objects);
        }
        
        var feedItems = new List<object>(items.Count);
        
        foreach (var item in items)
        {
            var feedItem = new Dictionary<string, object?>(16);
            
            AddIfNotNull(feedItem, "id", item.Id);
            AddIfNotNull(feedItem, "content_html", item.Content);
            AddIfNotNull(feedItem, "url", item.Link);
            AddIfNotNull(feedItem, "title", item.Title);
            AddIfNotNull(feedItem, "summary", item.Description);
            
            if (item.Image != null)
            {
                var img = new
                {
                    type = item.Image.Type,
                    url = item.Image.Url,
                    title = item.Image.Title
                };
                AddIfNotNull(feedItem, "image", img);
            } 
            
            AddIfNotNull(feedItem, "date_modified", Utils.ToIsoString(item.Date));

            var published = item.Published.HasValue ? Utils.ToIsoString(item.Published.Value) : null;
            AddIfNotNull(feedItem, "date_published", published);

            var author = item.Author.FirstOrDefault();
            var feedItemAuthor = author is null ? null : new { name = author.Name, url = author.Link };
            AddIfNotNull(feedItem, "author", feedItemAuthor);

            var tags = item.Category.Where(x => x.Name != null).Select(x => x.Name).ToArray();
            if (tags.Length > 0)
                AddIfNotNull(feedItem, "tags", tags);

            foreach (var itemExtension in item.Extensions)
            {
                AddIfNotNull(feedItem, itemExtension.Name, itemExtension.Objects);
            }
            
            feedItems.Add(feedItem);
        }
        
        feed["items"] = feedItems;

        using var ms = new MemoryStream(1024);
        await JsonSerializer.SerializeAsync(ms, feed, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
        ms.Position = 0;
        using var reader = new StreamReader(ms);
        var json = await reader.ReadToEndAsync();
        return json;
    }

    private static void AddIfNotNull(IDictionary<string, object?> dict, string name, object? val)
    {
        if (val is null)
        {
            return;
        }
        dict.Add(name, val);
    }
}