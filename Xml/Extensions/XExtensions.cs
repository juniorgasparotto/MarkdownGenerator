using SysCommand.ConsoleApp.Helpers;
using System.IO;
using System.Linq;
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
