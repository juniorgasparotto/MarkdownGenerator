using MarkdownGenerator.Helpers;
using SysCommand.ConsoleApp.Helpers;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MarkdownGenerator.Xml.Extensions
{
    /// <summary>
    /// Linq to XML XInclude extentions
    /// </summary>
    public static class XContent
    {
        /// <summary>
        /// Include element name
        /// </summary>
        public static readonly XName IncludeElementName = "include";

        /// <summary>
        /// Include location attribute
        /// </summary>
        public const string IncludeLocationAttributeName = "href";

        public static string GetContent(XElement xmlElement)
        {
            var strBuilder = new StringBuilder();

            foreach (var e in xmlElement.Descendants("content").ToList())
            {
                var newElements = XElement.Parse(StringHelper.TrimAllLines(e.OuterXml()).Trim(), LoadOptions.PreserveWhitespace);
                e.ReplaceNodes(newElements.Nodes());
            }

            ParseContent(xmlElement, strBuilder);
            return strBuilder.ToString();
        }
        
        private static void ParseContent(this XElement xmlElement, StringBuilder strBuilder)
        {
            var results = xmlElement.DescendantsAndSelf(IncludeElementName).ToArray();    // must be materialized

            foreach (var node in xmlElement.Elements("content").Nodes())
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Text:
                        strBuilder.Append(node.ToString());
                        break;
                    case XmlNodeType.Element:
                        var el = (XElement)node;
                        if (el.Name == IncludeElementName)
                        {
                            var path = el.Attribute(IncludeLocationAttributeName).Value;
                            path = Path.GetFullPath(path);
                            var content = FileHelper.GetContentFromFile(path);
                            strBuilder.AppendLine(content);
                            strBuilder.AppendLine();
                        }
                        else
                        {
                            strBuilder.Append(node.ToString());
                        }
                        break;
                }
            }
        }
    }
}
