# FeedSharp

A feed generator for C#/.NET.

## Support 

- [x] Atom 1.0
- [ ] RSS 2.0

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
mockFeed.AddCategory("bar");

mockFeed.AddItem(new Item("foo", "https://example.com/foo", new DateTime(2023, 1, 1))
{
	Description = "foo post"
});
mockFeed.AddItem(new Item("bar", "https://example.com/bar", new DateTime(2023, 1, 2))
{
	Description = "bar post"
});
```