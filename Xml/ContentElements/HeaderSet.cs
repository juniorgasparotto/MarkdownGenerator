using System.Xml.Linq;

namespace MarkdownMerge.Xml.Content
{
    public class HeaderSet : AnchorSet
    {
        public int Heading { get; private set; }

        public HeaderSet(Version version, XElement element, bool translated)
            : base(version, element, translated)
        {
            this.Heading = int.Parse(element.Attribute("heading").Value);
        }
    }
}