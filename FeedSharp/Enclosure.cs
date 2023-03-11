namespace FeedSharp;

public class Enclosure
{
    public Enclosure(string url)
    {
        Url = url;
    }
    
    public string Url { get; set; }
    public string? Type { get; set; }
    public int? Length { get; set; }
    public string? Title { get; set; }
    public int? Duration { get; set; }
}