using Html2Markdown.Replacement;
using HtmlAgilityPack;
using MarkdownGenerator.Helpers;
using System;
using System.Linq;

namespace MarkdownGenerator.Xml.Tags
{
    public class AnchorGet : NodeBase
    {
        public string Name { get; private set; }
        public string CustomText { get; private set; }

        public AnchorGet(Version version, HtmlNode element)
            : base(version, element)
        {
            this.Name = element.Attributes["name"].Value;
            this.CustomText = element.InnerHtml;
        }

        public override void ReplaceToMarkdown()
        {
            HtmlParser.ReplaceNode(Node, ToString());
        }

        public override string ToString()
        {
            // recupera apenas as versoes da mesma lingua em outras páginas
            var allPageVersion = Version.Page.Documentation.GetVersionsFromLanguage(Version.Language);
            var anchors = allPageVersion.SelectMany(f => f.Nodes).OfType<IAnchor>();

            var anchor = anchors.FirstOrDefault(f => f.Name == Name);
            if (anchor != null)
            {
                var text = string.IsNullOrWhiteSpace(CustomText) ? anchor.Text : CustomText;
                return MarkdownHelper.GetLinkFromAnchor(text, anchor.GetAnchorLink());
            }
            else
            {
                var exceptionDesc = $"The anchor '{Name}' doesn't exist for language version {Version.Language.Name}: {Node.ToString()}";
                return "<error>" + exceptionDesc + "</error>";
                throw new Exception(exceptionDesc);
            }
        }

       
    }
}