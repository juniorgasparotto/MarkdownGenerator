using MarkdownMerge.Helpers;
using System.Xml.Linq;
using System;
using System.IO;
using System.Linq;

namespace MarkdownMerge.Xml.Content
{
    public class AnchorSet : NodeBase
    {
        public string Name { get; private set; }
        public string Text { get; private set; }

        public AnchorSet(Version version, XElement element, bool translated)
            : base(version, element, translated)
        {
            this.Name = element.Attribute("name").Value;
            this.Text = element.Value;
        }

        public string GetAnchorLink()
        {
            var path = AppendUri(new Uri(Version.Page.Documentation.UrlBase), Version.Language.Output);
            return path + "#" + Name;
        }

        public static Uri AppendUri(Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }

        public override string ToString()
        {
            return MarkdownHelper.GetAnchor(Name, GetTranslate(Text));
        }
    }
}