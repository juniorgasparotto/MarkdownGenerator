using Html2Markdown.Replacement;
using HtmlAgilityPack;
using MarkdownMerge.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownMerge.Xml.Content
{
    public class TableOfContents : NodeBase
    {
        public TableOfContents(Version version, HtmlNode element) 
            : base(version, element)
        {
            
        }

        public override string ToString()
        {
            var headers = Version.Nodes.OfType<HeaderSet>();
            return GetTableOfContents(headers);
        }

        public string GetTableOfContents(IEnumerable<HeaderSet> headers)
        {
            var strBuilder = new StringBuilder();
            foreach (var header in headers)
            {
                if (header.Heading > 1)
                    strBuilder.Append(new String(' ', (header.Heading - 1) * 2));

                strBuilder.Append("*");
                strBuilder.Append(" " + MarkdownHelper.GetLinkFromAnchor(header.Text, header.GetAnchorLink()));
                if (header != headers.LastOrDefault())
                    strBuilder.AppendLine();
            }
            return strBuilder.ToString();
        }

        public override void Process()
        {
            if (Node.ParentNode.Name == "p")
                HtmlParser.ReplaceNode(Node.ParentNode, ToString());
            else
                HtmlParser.ReplaceNode(Node, ToString());
        }
    }
}