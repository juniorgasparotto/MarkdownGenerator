using MarkdownMerge.Translation;
using MarkdownMerge.Xml.Content;
using MarkdownMerge.Xml.Extensions;
using SysCommand.ConsoleApp;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MarkdownMerge.Xml
{
    public class Version
    {
        public ItemCollection Items { get; } = new ItemCollection();
        public Language Language { get; set; }
        public List<NodeBase> Nodes { get; } = new List<NodeBase>();
        public Page Page { get; private set; }
        public XElement XContent { get; internal set; }

        public Version(Page page)
        {
            this.Page = page;
        }

        public string GetMarkdown()
        {
            var xml = PrettyXml(XContent.OuterXml());
            return xml;
            //var strBuilder = new StringBuilder();
            //foreach (var element in Nodes)
            //    strBuilder.Append(element.ToString());
            //return strBuilder.ToString();
        }

        private string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = false;
            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }
}
