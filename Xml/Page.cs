using MarkdownMerge.Xml.Tags;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SysCommand.ConsoleApp.Helpers;
using MarkdownMerge.Xml.Extensions;
using HtmlAgilityPack;
using Html2Markdown.Replacement;

namespace MarkdownMerge.Xml
{
    public partial class Page
    {
        public List<Version> Versions { get; } = new List<Version>();
        public Documentation Documentation { get; private set; }

        public Page(Documentation doc, XElement xpage)
        {
            this.Documentation = doc;
            this.ParseVersions(xpage);
        }

        private void ParseVersions(XElement xpage)
        {
            var languages = xpage.Elements("languages");
            if (languages != null)
            {
                foreach (var xlang in languages.First().Elements("language"))
                {
                    var lang = new Language()
                    {
                        Name = xlang.Attribute("name").Value,
                        Output = xlang.Attribute("output").Value,
                        IsDefault = xlang.Attribute("default")?.Value == "true"
                    };

                    new Version(this, lang, xpage.Element("content").InnerXml());
                }
            }
        }

        public Version GetDefaultVersion()
        {
            return this.Versions.First(f => f.Language.IsDefault);
        }

        public void Save()
        {
            foreach (var version in Versions)
                FileHelper.SaveContentToFile(version.GetMarkdown(), version.Language.Output);
        }
    }
}
