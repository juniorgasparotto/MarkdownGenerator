using MarkdownMerge.Translation;
using MarkdownMerge.Xml.Content;
using MarkdownMerge.Xml.Extensions;
using SysCommand.ConsoleApp;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System;
using HtmlAgilityPack;

namespace MarkdownMerge.Xml
{
    public class Version
    {
        public Language Language { get; set; }
        public List<NodeBase> Nodes { get; } = new List<NodeBase>();
        public Page Page { get; private set; }
        public HtmlDocument XContent { get; internal set; }
        public string Content { get; internal set; }

        public Version(Page page)
        {
            this.Page = page;
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
