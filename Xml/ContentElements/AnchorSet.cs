using MarkdownMerge.Helpers;
using System.Xml.Linq;
using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using Html2Markdown.Replacement;

namespace MarkdownMerge.Xml.Content
{
    public class AnchorSet : NodeBase, IAnchor
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public AnchorSet(Version version, HtmlNode element)
            : base(version, element)
        {
            this.Name = element.Attributes["name"].Value;
            this.Text = element.InnerHtml;
        }

        public string GetAnchorLink()
        {
            var path = StringHelper.AppendUri(new Uri(Version.Page.Documentation.UrlBase), Version.Language.Output);
            return path + "#" + Name;
        }

        public override void Process()
        {
            HtmlParser.ReplaceNode(Node, ToString());
        }

        public override string ToString()
        {
            return MarkdownHelper.GetAnchor(Name, Text);
        }
    }
}