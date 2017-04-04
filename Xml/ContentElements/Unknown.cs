using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MarkdownMerge.Xml.Content
{
    public class Unknown : NodeBase
    {
        public Unknown(Version version, XElement element, bool translated) 
            : base(version, element, translated)
        {
        }

        public override string ToString()
        {
            var xElement = (XElement)Node;
            var xElementNew = new XElement(xElement);
            xElementNew.Value = GetTranslate(xElementNew.Value);
            return xElementNew.ToString();
        }
    }
}