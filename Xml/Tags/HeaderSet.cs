using Html2Markdown.Replacement;
using HtmlAgilityPack;
using MarkdownGenerator.Helpers;
using System;
using System.Collections.Generic;

namespace MarkdownGenerator.Xml.Tags
{
    public class HeaderSet : NodeBase, IAnchor
    {
        public int Heading { get; private set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public HeaderSet(Version version, HtmlNode element)
            : base(version, element)
        {
            var tag = element.ParentNode.Name;
            var strings = new List<string> { "h1", "h2", "h3", "h4", "h5", "h6" };
            if (strings.Contains(tag))
            {
                this.Heading = int.Parse(element.ParentNode.Name.Replace("h", ""));
                this.Name = element.Attributes["anchor-name"].Value;
                this.Text = element.ParentNode.InnerText.Trim();
            }
        }

        public string GetAnchorLink()
        {
            var path = StringHelper.AppendUri(new Uri(Version.Page.Documentation.UrlBase), Version.Language.Output);
            return path + "#" + Name;
        }

        public override void ReplaceToMarkdown()
        {
            Node.ParentNode.InnerHtml = ToString();
            Node.Remove();
        }

        public override string ToString()
        {
            return MarkdownHelper.GetAnchor(Name, Text);
        }
    }
}