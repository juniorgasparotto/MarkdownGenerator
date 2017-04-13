using MarkdownGenerator.Helpers;
using MarkdownGenerator.Xml.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MarkdownGenerator.Xml
{
    public class Documentation
    {
        public string UrlBase { get; set; }
        public List<Page> Pages { get; } = new List<Page>();

        public Documentation(string xmlConfig)
        {
            var xml = XDocument.Load(xmlConfig, LoadOptions.None);

            foreach (var e in xml.Descendants("content").ToList())
            {
                var newElements = XElement.Parse(StringHelper.TrimAllLines(e.OuterXml()).Trim());
                e.ReplaceNodes(newElements);
            }

            XInclude.ReplaceXIncludes(xml);
            var xdoc = xml.Root;
            this.UrlBase = xdoc.Attribute("url-base")?.Value;

            foreach (var xpage in xdoc.Elements("page"))
                this.Pages.Add(new Page(this, xpage));
        }

        public void Save()
        {
            foreach (var page in Pages)
                page.Save();
        }

        public IEnumerable<Version> GetVersionsFromLanguage(Language language)
        {
            var sameLangs = new List<Version>();
            foreach (var page in Pages)
            {
                var sameLang = page.Versions.FirstOrDefault(f => f.Language.Name == language.Name);
                if (sameLang != null)
                    sameLangs.Add(sameLang);
            }
            return sameLangs;
        }
    }
}
