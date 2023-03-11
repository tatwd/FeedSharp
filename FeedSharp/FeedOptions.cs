namespace FeedSharp;

public class FeedOptions
{
    public FeedOptions(string id, string title)
    {
        Id = id;
        Title = title;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public DateTime? Updated { get; set; }
    public string? Generator { get; set; }
    public string? Language { get; set; }
    public int? Ttl { get; set; }
    
    public string? Feed { get; set; }
    public FeedLinks? FeedLinks { get; set; }
    public string? Hub { get; set; }
    public string? Docs { get; set; }

    public Author? Author { get; set; }
    public string? Link { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Favicon { get; set; }
    public string? Copyright { get; set; }
}