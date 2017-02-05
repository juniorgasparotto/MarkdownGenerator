using System;

namespace MarkdownMerge.Commands
{
    public class XmlMethod
    {
        public string Name { get; set; }
        public Func<XmlMethodPart, string> Method { get; set; }
        public int Priority { get; set; }

        public XmlMethod(string name, Func<XmlMethodPart, string> xmlMethod, int priority)
        {
            this.Name = name;
            this.Method = xmlMethod;
            this.Priority = priority;
        }
    }
}
