using SysCommand.ConsoleApp.Helpers;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// Defines the maximum sub include count of 25
        /// </summary>
        public const int MaxSubIncludeCountDefault = 25;

        #endregion

        #region methods


        /// <summary>
        /// Replaces XInclude references with the target content.
        /// W3C Standard: http://www.w3.org/2003/XInclude
        /// </summary>
        /// <param name="xDoc">The xml doc.</param>
        /// <param name="maxSubIncludeCount">The max. allowed nested xml includes (default: 25).</param>
        public static void ReplaceXIncludes(this XDocument xDoc, int maxSubIncludeCount = MaxSubIncludeCountDefault)
        {
            ReplaceXIncludes(xDoc.Root, maxSubIncludeCount);
        }

        /// <summary>
        /// Replaces XInclude references with the target content.
        /// W3C Standard: http://www.w3.org/2003/XInclude
        /// </summary>
        /// <param name="xmlElement">The XML element.</param>
        /// <param name="maxSubIncludeCount">The max. allowed nested xml includes (default: 25).</param>
        public static void ReplaceXIncludes(this XElement xmlElement, int maxSubIncludeCount = MaxSubIncludeCountDefault)
        {
            xmlElement.ReplaceXIncludes(1, maxSubIncludeCount);
        }

        private static void ReplaceXIncludes(this XElement xmlElement, int subIncludeCount, int maxSubIncludeCount)
        {
            var results = xmlElement.DescendantsAndSelf(IncludeElementName).ToArray<XElement>();    // must be materialized

            foreach (var includeElement in results)
            {
                var path = includeElement.Attribute(IncludeLocationAttributeName).Value;
                path = Path.GetFullPath(path);
                var content = "<content>" + FileHelper.GetContentFromFile(path) + "</content>";
                var doc = XDocument.Parse(content, LoadOptions.PreserveWhitespace);
                if (subIncludeCount <= maxSubIncludeCount)  // protect mutal endless references
                {
                    // replace nested includes
                    doc.Root.ReplaceXIncludes(++subIncludeCount, maxSubIncludeCount);
                }
                includeElement.ReplaceWith(((XElement)doc.FirstNode).Nodes());
            }
        }

        #endregion
    }
}
