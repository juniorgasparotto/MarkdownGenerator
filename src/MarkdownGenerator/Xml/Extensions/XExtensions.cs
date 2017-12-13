using System.Xml.Linq;

namespace MarkdownGenerator.Xml.Extensions
{
    /// <summary>
    /// Linq to XML XInclude extentions
    /// </summary>
    public static class XExtensions
    {
        public static string InnerXml(this XNode node)
        {
            using (var reader = node.CreateReader())
            {
                reader.MoveToContent();
                return reader.ReadInnerXml();
            }
        }

        public static void ReplaceContent(this XElement element, string newContent)
        {
            var el = XElement.Parse(newContent);
            element.ReplaceNodes(el);
        }

        public static string OuterXml(this XNode node)
        {
            using (var reader = node.CreateReader())
            {
                reader.MoveToContent();
                return reader.ReadOuterXml();
            }
        }
    }
}
