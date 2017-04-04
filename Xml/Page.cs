using MarkdownMerge.Xml.Content;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System;
using SysCommand.ConsoleApp.Helpers;

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
                this.ParseContent(xpage.Element("content"), version, true);
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
                FileHelper.SaveContentToFile(version.GetContent(), version.Language.Output);
        }
    }
}
