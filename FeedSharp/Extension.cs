namespace FeedSharp;

public class Extension
{
    public Extension(string name, object objects)
    {
        Name = name;
        Objects = objects;
    }

    public string Name { get; set; }
    public object Objects { get; set; }
}