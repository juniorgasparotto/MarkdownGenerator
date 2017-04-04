using MarkdownMerge.Translation;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MarkdownMerge.Xml.Content
{
    public abstract class NodeBase
    {
        public Version Version { get; private set; }
        public XNode Node { get; private set; }
        public bool Translated { get; private set; }

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

        public NodeBase(Version version, XNode xnode, bool translated)
        {
            this.Version = version;
            this.Version.Nodes.Add(this);
            this.Node = xnode;
            this.Translated = translated;
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

        public string GetTranslate(string text)
        {
            var fromLang = Version.Page.GetDefaultVersion().Language.Name;
            var toLang = Version.Language.Name;

            if (!Translated || fromLang == toLang)
                return text;

            var textTranslated = Translator.Translate(text, fromLang, toLang);
            return textTranslated;
        }

        public override string ToString()
        {
            var str = Node.ToString();
            return str;
        }
    }
}
