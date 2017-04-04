using MarkdownMerge.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MarkdownMerge.Xml.Content
{
    public class AnchorGet : NodeBase
    {
        public string Name { get; private set; }
        public string CustomText { get; private set; }

        public AnchorGet(Version version, XElement element, bool translated)
            : base(version, element, translated)
        {
            this.Name = element.Attribute("name").Value;
            this.CustomText = element.Value;
        }


        public override string ToString()
        {
            // recupera apenas as versoes da mesma lingua em outras páginas
            var allPageVersion = Version.Page.Documentation.GetVersionsFromLanguage(Version.Language);
            var anchors = allPageVersion.SelectMany(f => f.Nodes).OfType<AnchorSet>();

            var anchor = anchors.FirstOrDefault(f => f.Name == Name);
            if (anchor != null)
            {
                var text = string.IsNullOrWhiteSpace(CustomText) ? anchor.Text : CustomText;
                return MarkdownHelper.GetLinkFromAnchor(GetTranslate(text), anchor.GetAnchorLink());
            }
            else
            {
                throw new Exception($"The anchor '{Name}' doesn't exist for language version {Version.Language.Name}: {Node.ToString()}");
            }
        }
    }
}