# FeedSharp

A feed generator for C#/.NET. This project is inspired [jpmonette/feed](https://github.com/jpmonette/feed).

## Support 

- [x] Atom 1.0
- [x] RSS 2.0
- [x] JSON Feed 1.0

## Install

```
dotnet add package FeedSharp --version 0.1.4
```

## Example

```csharp
var feed = new Feed(new FeedOptions("https://example.com/", "Hello FeedSharp")
{
    // these props are optional
    Description = "this is a description",
    Link = "https://example.com/",
    Language = "zh", 
    Image = "http://example.com/image.png",
    Favicon = "http://example.com/favicon.ico",
    Copyright = "All rights reserved 2013, _king",
    Author = new Author
    {
        Name = "_king",
        Email = "tatwd@exmaple.com",
        Link = "https://example.com/tatwd"
    },
    Feed = "https://example.com/feed",
    FeedLinks = new FeedLinks
    {
        Atom = "https://example.com/atom.xml",
        Rss = "https://example.com/rss.xml",
        Json = "https://example.com/feed.json"
    },
    Hub = "https://example.com/hub",
    Generator = "awesome",
    Updated = DateTime.Now
});
feed.AddContributor(new Author
{
    Name = "foo",
    Email = "foo@exmaple.com",
    Link = "https://example.com/foo"
});
feed.AddCategory("foo");
feed.AddItem(new Item("foo", "https://example.com/foo", new DateTime(2023, 1, 1))
{
    // these props are optional
    Description = "foo post",
    Content = "foo content",
    Image = new Enclosure("https://exmaple.com/img.png"),
    Category = new[] 
    { 
        new Category { Name = "foo" },
        new Category { Term = "bar" } 
    }
});
feed.AddExtension(new Extension("foo_ext", "hello"));
feed.AddExtension(new Extension("bar_ext", new { baz = true }));

// atom1
feed.ToAtom1();
// or
await feed.ToAtom1Async();

// rss2
feed.ToRss2();
// or
await feed.ToRss2Async();

// json1
feed.ToJson1();
// or
await feed.ToJson1Async();
```