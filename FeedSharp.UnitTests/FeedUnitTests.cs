namespace FeedSharp.UnitTests;

public class FeedUnitTests
{
    [Fact]
    public void ToAtom1_ok()
    {
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
        
        var atom1 = mockFeed.ToAtom1();

        const string expected = """
<?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom">
  <id>https://example.com/</id>
  <title>Hello FeedSharp</title>
  <updated>2023-03-01T04:00:00.000Z</updated>
  <generator>https://github.com/tatwd/FeedSharp</generator>
  <author>
    <name>_king</name>
    <email>tatwd@exmaple.com</email>
  </author>
  <link rel="alternate" href="https://example.com/" />
  <subtitle>this is a description</subtitle>
  <category term="foo" />
  <category term="bar" />
  <entry>
    <title type="html"><![CDATA[foo]]></title>
    <id>https://example.com/foo</id>
    <link href="https://example.com/foo" />
    <updated>2022-12-31T16:00:00.000Z</updated>
    <summary type="html"><![CDATA[foo post]]></summary>
  </entry>
  <entry>
    <title type="html"><![CDATA[bar]]></title>
    <id>https://example.com/bar</id>
    <link href="https://example.com/bar" />
    <updated>2023-01-01T16:00:00.000Z</updated>
    <summary type="html"><![CDATA[bar post]]></summary>
  </entry>
</feed>
""";
        Assert.Equal(expected, atom1);
    }
}