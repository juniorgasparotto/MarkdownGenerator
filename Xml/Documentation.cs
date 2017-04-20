using MarkdownGenerator.Translation;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MarkdownGenerator.Xml
{
    public class Documentation
    {
        public List<Page> Pages { get; } = new List<Page>();
        public Translator Translator { get; private set; }

        public Documentation(string xmlConfig, Translator translator)
        {
            this.Translator = translator;
            var xml = XDocument.Load(xmlConfig, LoadOptions.PreserveWhitespace);

            var xdoc = xml.Root;

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
