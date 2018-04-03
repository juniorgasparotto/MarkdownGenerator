using MarkdownGenerator.Helpers;
using System.Xml.Linq;
using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using Html2Markdown.Replacement;

namespace MarkdownGenerator.Xml.Tags
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
            var urlbase = Version.Language.UrlBase;
            return StringHelper.GetAnchorLink(urlbase, Name);
        }

        public override void ReplaceToMarkdown()
        {
            HtmlParser.ReplaceNode(Node, ToString());
        }

        public override string ToString()
        {
            return MarkdownHelper.GetAnchor(Name, Text);
        }
    }
}