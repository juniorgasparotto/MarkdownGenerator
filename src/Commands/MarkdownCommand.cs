using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using MarkdownGenerator.Xml;
using System.IO;
using HtmlAgilityPack;
using MarkdownGenerator.Translation;

namespace MarkdownGenerator.Commands
{
    public partial class MarkdownCommand : Command
    {
        public MarkdownCommand()
        {
            this.HelpText = "This tools include: Merge markdown files, Automatic translation with MS Translator, Generate automatic table of contents, Generate anchor reference";
            HtmlNode.ElementsFlags["a"] = HtmlElementFlag.Empty | HtmlElementFlag.Closed | HtmlElementFlag.CanOverlap;
        }

        public void Main(
            [Argument(Help = "The xml config file")]
            string indexFile,

            [Argument(Help = "Base directory to save outputs. If empty the current directory will be used")]
            string baseDir = null,

            [Argument(Help = "Set the Microsoft Translator Key if your document has more than one language versions")]
            string translatorKey = null
        )
        {
            //indexFile = "Sample/Index.xml";
            //baseDir = null;

            if (baseDir != null)
                Directory.SetCurrentDirectory(baseDir);
            var doc = new Documentation(indexFile, translatorKey);
            doc.Save();
        }
    }
}