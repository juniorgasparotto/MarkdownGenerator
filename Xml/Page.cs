using MarkdownMerge.Xml.Content;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System;
using SysCommand.ConsoleApp.Helpers;
using MarkdownMerge.Translation;
using MarkdownMerge.Xml.Extensions;

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
                version.XContent = CopyContent(xpage);
                PrepareContentToNoTranslations(version);
                TranslateContent(version);
                PrepareContentToCustomTranslation(version);
                //this.ParseContent(xpage.Element("content"), version, true);
            }
        }

        private XElement CopyContent(XElement xpage)
        {
            return CreateElement(xpage.Element("content").OuterXml());
        }

        private XElement CreateElement(string xml)
        {
            return XDocument.Parse(xml).Root;
        }

        private void TranslateContent(Version version)
        {
            var fromLang = GetDefaultVersion().Language.Name;
            var toLang = version.Language.Name;
            if (fromLang != toLang)
                version.XContent = CreateElement(Translator.Translate(version.XContent.OuterXml(), fromLang, toLang));
        }

        private void PrepareContentToCustomTranslation(Version version)
        {
            var xCustomTranslations = version.XContent.Elements(ElementNamesConstants.CustomTranslation);

            foreach (var customTranslation in xCustomTranslations)
            {
                if (version == GetDefaultVersion())
                {
                    var original = customTranslation.Element(ElementNamesConstants.CustomTranslationDefault);
                    customTranslation.ReplaceWith(original.Nodes());
                }
                else
                {
                    foreach (var lang in customTranslation.Elements(ElementNamesConstants.CustomTranslationLanguage))
                    {
                        if (lang.Attribute("name").Value == version.Language.Name)
                        {
                            customTranslation.ReplaceWith(lang.Nodes());
                            break;
                        }
                    }
                }
            }
        }

        private void PrepareContentToNoTranslations(Version version)
        {
            foreach (var e in version.XContent.Elements(ElementNamesConstants.NoTranslation))
                e.SetAttributeValue("class", "notranslate");

            foreach (var e in version.XContent.Elements(ElementNamesConstants.CustomTranslation))
                e.SetAttributeValue("class", "notranslate");
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

        private void ParseContent(XElement xCurrentNode, Version version, bool translated)
        {
            foreach (XNode xnode in xCurrentNode.Nodes())
            {
                if (xnode.NodeType == XmlNodeType.Element)
                {
                    var element = (XElement)xnode;
                    switch (element.Name.LocalName)
                    {
                        case ElementNamesConstants.AnchorSet:
                            new AnchorSet(version, element, translated);
                            break;
                        case ElementNamesConstants.AnchorGet:
                            new AnchorGet(version, element, translated);
                            break;
                        case ElementNamesConstants.HeaderSet:
                            new HeaderSet(version, element, translated);
                            break;
                        case ElementNamesConstants.TableOfContents:
                            new TableOfContents(version, element, translated);
                            break;
                        case ElementNamesConstants.NoTranslation:
                            ParseContent(element, version, false);
                            break;
                        case ElementNamesConstants.CustomTranslation:
                            if (version == GetDefaultVersion())
                            {
                                ParseContent(element.Element(ElementNamesConstants.CustomTranslationDefault), version, translated);
                            }
                            else
                            {
                                foreach (var lang in element.Elements(ElementNamesConstants.CustomTranslationLanguage))
                                {
                                    if (lang.Attribute("name").Value == version.Language.Name)
                                    { 
                                        ParseContent(lang, version, translated);
                                        break;
                                    }
                                }
                            }
                            break;
                        default:
                            new Unknown(version, element, translated);
                            break;
                    }
                }
                else if (xnode.NodeType == XmlNodeType.CDATA)
                {
                    new TextPlain(version, (XText)xnode, translated);
                }
                else if (xnode.NodeType == XmlNodeType.Text)
                {
                    new TextPlain(version, (XText)xnode, translated);
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
