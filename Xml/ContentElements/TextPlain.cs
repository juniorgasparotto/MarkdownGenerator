using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MarkdownMerge.Xml.Content
{
    public class TextPlain : NodeBase
    {
        public string Text { get; private set; }

        public TextPlain(Version version, XText element, bool translated) 
            : base(version, element, translated)
        {
            this.Text = element.Value;
        }

        public override string ToString()
        {
            return GetTranslate(Text);
        }
    }
}