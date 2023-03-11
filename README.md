# FeedSharp

A feed generator for C#/.NET. This project is inspired [jpmonette/feed](https://github.com/jpmonette/feed).

## Support 

- [x] Atom 1.0
- [x] RSS 2.0

## Install

```
dotnet add package FeedSharp --prerelease
```

## Example

```csharp
var mockFeed = new Feed(new FeedOptions("https://example.com/", "Hello FeedSharp")
{
    Description = "this is a description",
    Link = "https://example.com/",
    Author = new Author
    {
        Name = "_king",
        Email = "tatwd@exmaple.com"
    },
    Updated = new DateTime(2023, 3, 1, 12, 0, 0)
});
mockFeed.AddCategory("foo");
mockFeed.AddItem(new Item("foo", "https://example.com/foo", new DateTime(2023, 1, 1))
{
    Description = "foo post"
});

// atom1
mockFeed.ToAtom1();
// or
await mockFeed.ToAtom1Async();

// rss2
mockFeed.ToRss2();
// or
await mockFeed.ToRss2Async();
```