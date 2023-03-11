namespace FeedSharp;

public class Item
{
    public Item(string title, string link, DateTime date)
    {
        Title = title;
        Link = link;
        Date = date;
    }
    
    public string Title { get; set; }
    public string? Id { get; set; }
    public string Link { get; set; }
    public DateTime Date { get; set; }
    
    public string? Description { get; set; }
    public string? Content { get; set; }
    public Category[] Category { get; set; } = Array.Empty<Category>();
    
    public string? Guid { get; set; }
    
    public string? Image { get; set; }
    public string? Audio { get; set; }
    public string? Video { get; set; }
    public Enclosure? Enclosure { get; set; }

    public Author[] Author { get; set; } = Array.Empty<Author>();
    public Author[] Contributor { get; set; } = Array.Empty<Author>();
    
    public DateTime? Published { get; set; }
    public string? Copyright { get; set; }
    
    public Extension[]? Extensions { get; set; }
}