using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using MarkdownMerge.Xml;
using System.IO;
using HtmlAgilityPack;

namespace MarkdownMerge.Commands
{
    public partial class MarkdownCommand : Command
    {
        public MarkdownCommand()
        {
            this.HelpText = "Merge markdown files; Generate automatic table of contents; Generate anchor reference;";
            HtmlNode.ElementsFlags["a"] = HtmlElementFlag.Empty | HtmlElementFlag.Closed | HtmlElementFlag.CanOverlap;
        }

        public void Main(
            [Argument(Help = "Xml index file")]
            string indexFile = @"documentation\.generator\index.xml",

            [Argument(Help = "Base directory to save outputs")]
            string baseDir = @"D:\Junior\Projetos\GITHUB.COM\juniorgasparotto\SysCommand"
        )
        {
            indexFile = "Sample/Index.xml";
            baseDir = null;

            if (baseDir != null)
                Directory.SetCurrentDirectory(baseDir);

            var doc = new Documentation(indexFile);
            doc.Save();
        }
    }
}