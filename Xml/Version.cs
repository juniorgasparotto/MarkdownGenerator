using MarkdownMerge.Translation;
using MarkdownMerge.Xml.Content;
using SysCommand.ConsoleApp;
using System.Collections.Generic;
using System.Text;

namespace MarkdownMerge.Xml
{
    public class Version
    {
        public ItemCollection Items { get; } = new ItemCollection();
        public Language Language { get; set; }
        public List<NodeBase> Nodes { get; } = new List<NodeBase>();
        public Page Page { get; private set; }

        public Version(Page page)
        {
            this.Page = page;
        }

        public string GetContent()
        {
            var strBuilder = new StringBuilder();
            foreach (var element in Nodes)
                strBuilder.Append(element.ToString());
            return strBuilder.ToString();
        }

        
    }
}
