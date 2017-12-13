using MarkdownGenerator.Xml.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownGenerator.Helpers
{
    public static class MarkdownHelper
    {
        public static string GetLinkFromAnchor(string text, string anchor)
        {
            return $"[{text ?? anchor}]({anchor})";
        }

        public static string GetAnchor(string name, string text)
        {
            return $@"<a name=""{name}""></a>{text}";
        }

        
    }
}