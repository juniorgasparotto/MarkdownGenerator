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
        public HtmlNode ParentNode { get; private set; }

        public HeaderSet(Version version, HtmlNode element)
            : base(version, element)
        {
            ParentNode = element.ParentNode;
            var tag = element.ParentNode.Name;
            var strings = new List<string> { "h1", "h2", "h3", "h4", "h5", "h6" };
            if (strings.Contains(tag))
            {
                this.Heading = int.Parse(element.ParentNode.Name.Replace("h", ""));
                this.Name = element.Attributes["anchor-name"].Value;
                element.Remove();
                this.Text = ParentNode.InnerHtml.Trim();
            }
        }

        public string GetAnchorLink()
        {
            return StringHelper.GetAnchorLink(Version.Page.UrlBase, Version.Language.Output, Name);
        }

        public override void ReplaceToMarkdown()
        {
            if (!string.IsNullOrWhiteSpace(this.Text))
                ParentNode.InnerHtml = ToString();
        }

        public override string ToString()
        {
            return MarkdownHelper.GetAnchor(Name, Text);
        }
    }
}