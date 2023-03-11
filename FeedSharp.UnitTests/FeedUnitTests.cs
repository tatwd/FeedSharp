namespace FeedSharp.UnitTests;

public class FeedUnitTests
{
    [Fact]
    public void ToAtom1_mini_setup_without_items_ok()
    {
        var mockFeed = new Feed(new FeedOptions("https://example.com/", "Hello FeedSharp")
        {
            Updated = new DateTime(2023, 3, 1, 12, 0, 0, DateTimeKind.Utc)
        });
        var atom1 = mockFeed.ToAtom1();
        
        const string expected = """
<?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom">
  <id>https://example.com/</id>
  <title>Hello FeedSharp</title>
  <updated>2023-03-01T12:00:00.000Z</updated>
  <generator>https://github.com/tatwd/FeedSharp</generator>
</feed>
""";
        Assert.Equal(expected, atom1);
    }
    
    [Fact]
    public void ToAtom1_mini_setup_with_items_ok()
    {
        var mockFeed = new Feed(new FeedOptions("https://example.com/", "Hello FeedSharp")
        {
            Updated = new DateTime(2023, 3, 1, 12, 0, 0, DateTimeKind.Utc)
        });
        mockFeed.AddItem(new Item("foo", "https://example.com/foo", new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        var atom1 = mockFeed.ToAtom1();

        const string expected = """
<?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom">
  <id>https://example.com/</id>
  <title>Hello FeedSharp</title>
  <updated>2023-03-01T12:00:00.000Z</updated>
  <generator>https://github.com/tatwd/FeedSharp</generator>
  <entry>
    <title type="html"><![CDATA[foo]]></title>
    <id>https://example.com/foo</id>
    <link href="https://example.com/foo" />
    <updated>2023-01-01T00:00:00.000Z</updated>
  </entry>
</feed>
""";
        Assert.Equal(expected, atom1);
    }
    
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
            Updated = new DateTime(2023, 3, 1, 12, 0, 0, DateTimeKind.Utc)
        });
        mockFeed.AddCategory("foo");
        mockFeed.AddCategory("bar");

        mockFeed.AddItem(new Item("foo", "https://example.com/foo", new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc))
        {
            Description = "foo post"
        });
        mockFeed.AddItem(new Item("bar", "https://example.com/bar", new DateTime(2023, 1, 2, 0, 0, 0, DateTimeKind.Utc))
        {
            Description = "bar post"
        });
        
        var atom1 = mockFeed.ToAtom1();

        const string expected = """
<?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom">
  <id>https://example.com/</id>
  <title>Hello FeedSharp</title>
  <updated>2023-03-01T12:00:00.000Z</updated>
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
    <updated>2023-01-01T00:00:00.000Z</updated>
    <summary type="html"><![CDATA[foo post]]></summary>
  </entry>
  <entry>
    <title type="html"><![CDATA[bar]]></title>
    <id>https://example.com/bar</id>
    <link href="https://example.com/bar" />
    <updated>2023-01-02T00:00:00.000Z</updated>
    <summary type="html"><![CDATA[bar post]]></summary>
  </entry>
</feed>
""";
        Assert.Equal(expected, atom1);
    }
    
    
    [Fact]
    public void ToRss2_mini_setup_without_items_ok()
    {
        var mockFeed = new Feed(new FeedOptions("https://example.com/", "Hello FeedSharp")
        {
            Updated = new DateTime(2023, 3, 1, 12, 0, 0, DateTimeKind.Utc)
        });
        var rss2 = mockFeed.ToRss2();

        const string expected = """
<?xml version="1.0" encoding="utf-8"?>
<rss version="2.0">
  <channel>
    <title>Hello FeedSharp</title>
    <lastBuildDate>Wed, 01 Mar 2023 12:00:00 GMT</lastBuildDate>
    <docs>https://validator.w3.org/feed/docs/rss2.html</docs>
    <generator>https://github.com/tatwd/FeedSharp</generator>
  </channel>
</rss>
""";
        Assert.Equal(expected, rss2);
    }
    

    [Fact]
    public void ToRss2_mini_setup_with_items_ok()
    {
        var mockFeed = new Feed(new FeedOptions("https://example.com/", "Hello FeedSharp")
        {
            Updated = new DateTime(2023, 3, 1, 12, 0, 0, DateTimeKind.Utc)
        });
        mockFeed.AddItem(new Item("foo", "https://example.com/foo", new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        var rss2 = mockFeed.ToRss2();

        const string expected = """
<?xml version="1.0" encoding="utf-8"?>
<rss version="2.0">
  <channel>
    <title>Hello FeedSharp</title>
    <lastBuildDate>Wed, 01 Mar 2023 12:00:00 GMT</lastBuildDate>
    <docs>https://validator.w3.org/feed/docs/rss2.html</docs>
    <generator>https://github.com/tatwd/FeedSharp</generator>
    <item>
      <title><![CDATA[foo]]></title>
      <link>https://example.com/foo</link>
      <guid>https://example.com/foo</guid>
      <pubDate>Sun, 01 Jan 2023 00:00:00 GMT</pubDate>
    </item>
  </channel>
</rss>
""";
        Assert.Equal(expected, rss2);
    }
    
    [Fact]
    public void ToRss2_ok()
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
            Feed = "https://example.com/feed",
            Hub = "https://example.com/hub",
            Updated = new DateTime(2023, 3, 1, 12, 0, 0, DateTimeKind.Utc)
        });
        mockFeed.AddCategory("foo");
        mockFeed.AddCategory("bar");

        mockFeed.AddItem(new Item("foo", "https://example.com/foo", new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc))
        {
            Description = "foo post", Content = "foo content"
        });
        mockFeed.AddItem(new Item("bar", "https://example.com/bar", new DateTime(2023, 1, 2, 0, 0, 0, DateTimeKind.Utc))
        {
            Description = "bar post"
        });
        
        var rss2 = mockFeed.ToRss2();

        const string expected = """
<?xml version="1.0" encoding="utf-8"?>
<rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:content="http://purl.org/rss/1.0/modules/content/" xmlns:atom="http://www.w3.org/2005/Atom">
  <channel>
    <title>Hello FeedSharp</title>
    <link>https://example.com/</link>
    <description>this is a description</description>
    <lastBuildDate>Wed, 01 Mar 2023 12:00:00 GMT</lastBuildDate>
    <docs>https://validator.w3.org/feed/docs/rss2.html</docs>
    <generator>https://github.com/tatwd/FeedSharp</generator>
    <category>foo</category>
    <category>bar</category>
    <atom:link href="https://example.com/hub" rel="hub" />
    <item>
      <title><![CDATA[foo]]></title>
      <link>https://example.com/foo</link>
      <guid>https://example.com/foo</guid>
      <pubDate>Sun, 01 Jan 2023 00:00:00 GMT</pubDate>
      <description><![CDATA[foo post]]></description>
      <content:encoded><![CDATA[foo content]]></content:encoded>
    </item>
    <item>
      <title><![CDATA[bar]]></title>
      <link>https://example.com/bar</link>
      <guid>https://example.com/bar</guid>
      <pubDate>Mon, 02 Jan 2023 00:00:00 GMT</pubDate>
      <description><![CDATA[bar post]]></description>
    </item>
  </channel>
</rss>
""";
        Assert.Equal(expected, rss2);
    }
}