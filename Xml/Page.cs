using MarkdownMerge.Xml.Content;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SysCommand.ConsoleApp.Helpers;
using MarkdownMerge.Translation;
using MarkdownMerge.Xml.Extensions;
using Markdig;
using Html2Markdown;
using HtmlAgilityPack;
using Html2Markdown.Replacement;
using System;

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

            foreach(var version in Versions)
            {
                var markdown = xpage.Element("content").InnerXml();
                markdown = PrepareHtml(markdown);
                var html = MarkdownToHtml(markdown);
                //html = TranslateHtml(html, version.Language.Name);
                version.XContent = HtmlParser.GetHtmlDocument(html);
                ParseElements(version);
            }
        }

        private string PrepareHtml(string html)
        {
            var htmlObj = HtmlParser.GetHtmlDocument(html);

            // remove spaces in no-translatin itens
            var noTranslations = htmlObj.DocumentNode.Elements(ElementNamesConstants.NoTranslation);
            if (noTranslations != null)
            { 
                foreach (var no in noTranslations)
                {
                    no.Attributes.Add("class", ElementNamesConstants.NotTranslateDefinition);
                    //no.InnerHtml = no.InnerHtml.Trim();
                }
            }

            // add notranslate attr in custom translations
            var customTransaltions = htmlObj.DocumentNode.Elements(ElementNamesConstants.CustomTranslation);
            if (customTransaltions != null)
            { 
                if (customTransaltions != null)
                    foreach (var custom in customTransaltions)
                        custom.Attributes.Add("class", ElementNamesConstants.NotTranslateDefinition);
            }

            return htmlObj.DocumentNode.OuterHtml;
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

                    this.Versions.Add(new Version(this) { Language = lang });
                }
            }
        }

        private void ParseElements(Version version)
        {
            ParseNoTranslate(version.XContent.DocumentNode.SelectNodes("//" + ElementNamesConstants.NoTranslation));
            ParseCustomTranslation(version.XContent.DocumentNode.SelectNodes("//" + ElementNamesConstants.CustomTranslation)?.ToArray(), version);

            ParseElements(version.XContent.DocumentNode.SelectNodes("//" + ElementNamesConstants.HeaderSet)?.ToArray(), version);
            ParseElements(version.XContent.DocumentNode.SelectNodes("//" + ElementNamesConstants.AnchorSet)?.ToArray(), version);
            ParseElements(version.XContent.DocumentNode.SelectNodes("//" + ElementNamesConstants.AnchorGet)?.ToArray(), version);
            ParseElements(version.XContent.DocumentNode.SelectNodes("//" + ElementNamesConstants.TableOfContents)?.ToArray(), version);
        }

        private void ParseElements(IEnumerable<HtmlNode> elements, Version version)
        {
            if (elements == null)
                return;

            foreach (var element in elements)
            {
                switch (element.Name)
                {
                    case ElementNamesConstants.AnchorSet:
                        new AnchorSet(version, element);
                        break;
                    case ElementNamesConstants.AnchorGet:
                        new AnchorGet(version, element);
                        break;
                    case ElementNamesConstants.HeaderSet:
                        new HeaderSet(version, element);
                        break;
                    case ElementNamesConstants.TableOfContents:
                        new TableOfContents(version, element);
                        break;
                }
            }
        }

        private void ParseNoTranslate(IEnumerable<HtmlNode> elements)
        {
            if (elements == null)
                return;

            foreach (var element in elements)
                HtmlParser.ReplaceNode(element, MarkdownToHtml(element.InnerHtml.Trim()));
        }

        private void ParseCustomTranslation(IEnumerable<HtmlNode> elements, Version version)
        {
            if (elements == null)
                return;

            foreach (var element in elements)
            {
                if (version == GetDefaultVersion())
                {
                    var original = element.Element(ElementNamesConstants.CustomTranslationDefault);
                    HtmlParser.ReplaceNode(element, original.InnerHtml.Trim());
                }
                else
                {
                    foreach (var lang in element.Elements(ElementNamesConstants.CustomTranslationLanguage))
                    {
                        if (lang.Attributes["name"].Value == version.Language.Name)
                        {
                            HtmlParser.ReplaceNode(element, lang.InnerHtml.Trim());
                            break;
                        }
                    }
                }
            }
        }

        public void ProcessElements()
        {
            foreach (var version in Versions)
            {
                foreach (var node in version.Nodes)
                    node.Process();

                version.Content = HtmlToMarkdown(version.XContent.DocumentNode.OuterHtml);
            }
        }

        private string HtmlToMarkdown(string html)
        {
            var converter = new Converter();
            return converter.Convert(html);
        }

        private string MarkdownToHtml(string markdown)
        {
            return Markdown.ToHtml(markdown);
        }

        private string TranslateHtml(string html, string toLang)
        {   
            var fromLang = GetDefaultVersion().Language.Name;
            if (fromLang != toLang)
                return Translator.Translate(html, fromLang, toLang);
            return html;
        }

        public Version GetDefaultVersion()
        {
            return this.Versions.First(f => f.Language.IsDefault);
        }

        public void Save()
        {
            foreach (var version in Versions)
                FileHelper.SaveContentToFile(version.Content, version.Language.Output);
        }
    }
}
