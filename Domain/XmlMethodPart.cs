using System.Collections.Generic;
using System;
using System.Xml;

namespace MarkdownMerge.Commands
{
    public class XmlMethodPart : StringPart
    {
        public XmlNode Node { get; private set; }
        public XmlMethod XmlMethod { get; private set; }
        private string result;

        public XmlMethodPart(List<StringPart> parts, string realString, XmlNode node, XmlMethod xmlMethod)
            : base(parts, realString)
        {
            this.Node = node;
            this.XmlMethod = xmlMethod;
        }

        public override void Process()
        {
            this.result = this.XmlMethod.Method(this);
        }

        public override string ToString()
        {
            return this.result;
        }
    }
}
