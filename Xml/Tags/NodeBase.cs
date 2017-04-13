using HtmlAgilityPack;
using MarkdownGenerator.Translation;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MarkdownGenerator.Xml.Tags
{
    public abstract class NodeBase
    {
        public Version Version { get; private set; }
        public HtmlNode Node { get; private set; }

        public NodeBase Before
        {
            get
            {
                return this.Version.Nodes.ElementAtOrDefault(this.Version.Nodes.IndexOf(this) - 1);
            }
        }

        public NodeBase Next
        {
            get
            {
                return this.Version.Nodes.ElementAtOrDefault(this.Version.Nodes.IndexOf(this) + 1);
            }
        }

        public NodeBase(Version version, HtmlNode node)
        {
            this.Version = version;
            this.Version.Nodes.Add(this);
            this.Node = node;
        }

        public T GetBefore<T>() where T : NodeBase
        {
            var before = Before;
            if (before != null && before is T)
                return (T)before;
            return null;
        }

        public T GetNext<T>() where T : NodeBase
        {
            var next = Next;
            if (next != null && next is T)
                return (T)next;
            return null;
        }

        //public string GetTranslate(string text)
        //{
        //    var fromLang = Version.Page.GetDefaultVersion().Language.Name;
        //    var toLang = Version.Language.Name;

        //    var textTranslated = Translator.Translate(text, fromLang, toLang);
        //    return textTranslated;
        //}

        public abstract void ReplaceToMarkdown();

        public override string ToString()
        {
            var str = Node.ToString();
            return str;
        }
    }
}
