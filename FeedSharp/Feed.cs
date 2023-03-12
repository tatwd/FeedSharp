using FeedSharp.Internals;

namespace FeedSharp;

public sealed class Feed
{
    public readonly FeedOptions Options;
    public readonly List<Item> Items = new(16);
    public readonly List<string> Categories = new();
    public readonly List<Author> Contributors = new();
    public readonly List<Extension> Extensions = new();
    
    public Feed(FeedOptions options)
    {
        Options = options;
    }

    public void AddItem(Item item) => Items.Add(item);
    
    public void AddCategory(string category) => Categories.Add(category);
    
    public void AddContributor(Author contributor) => Contributors.Add(contributor);
    
    public void AddExtension(Extension extension) => Extensions.Add(extension);

    public string ToAtom1() => new Atom1(this).Render();
    
    public Task<string> ToAtom1Async() => new Atom1(this).RenderAsync();

    public string ToRss2() => new Rss2(this).Render();
    
    public Task<string> ToRss2Async() => new Rss2(this).RenderAsync();
    
    public string ToJson1() => new Json1(this).Render();
    
    public Task<string> ToJson1Async() => new Json1(this).RenderAsync();
}