using System.Text;

namespace FeedSharp.Internals;

internal class Utf8StringWriter : StringWriter
{
    public Utf8StringWriter() { }

    public override Encoding Encoding => Encoding.UTF8;
}