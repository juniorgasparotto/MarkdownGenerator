using SysCommand.ConsoleApp.Helpers;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MarkdownGenerator.Xml.Extensions
{
    /// <summary>
    /// Linq to XML XInclude extentions
    /// </summary>
    public static class XInclude
    {
        #region fields

        /// <summary>
        /// W3C XInclude standard
        /// Be aware of the different 2001 and 2003 standard.
        /// </summary>
        public static readonly XNamespace IncludeNamespace = ""; // "http://www.w3.org/2003/XInclude";

        /// <summary>
        /// Include element name
        /// </summary>
        public static readonly XName IncludeElementName = IncludeNamespace + "include";

        /// <summary>
        /// Include location attribute
        /// </summary>
        public const string IncludeLocationAttributeName = "href";

        #endregion

        #region methods
        
        public static string GetXmlIncludeTag(this XElement xmlElement)
        {
            var strBuilder = new StringBuilder();
            ParseXmlIncludeTag(xmlElement, strBuilder);
            return strBuilder.ToString();
        }
        
        private static void ParseXmlIncludeTag(this XElement xmlElement, StringBuilder strBuilder)
        {
            var results = xmlElement.DescendantsAndSelf(IncludeElementName).ToArray();    // must be materialized

            foreach (var includeElement in results)
            {
                var path = includeElement.Attribute(IncludeLocationAttributeName).Value;
                path = Path.GetFullPath(path);
                var content = FileHelper.GetContentFromFile(path);
                strBuilder.AppendLine(content);
                strBuilder.AppendLine();
            }
        }

        #endregion
    }
}
