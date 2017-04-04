using SysCommand.ConsoleApp;
using SysCommand.ConsoleApp.Helpers;
using System.Collections.Generic;
using System.Linq;
using SysCommand.Mapping;
using MarkdownMerge.Xml.Content;
using MarkdownMerge.Xml;

namespace MarkdownMerge.Commands
{
    public partial class MarkdownCommand : Command
    {
        public MarkdownCommand()
        {
            this.HelpText = "Merge markdown files; Generate automatic table of contents; Generate anchor reference;";
        }

        //public string Main(
        //    [Argument(LongName = "files-index", ShortName = 'i', Help = "Markdown file that represents the index of your documents")]
        //    List<string> filesIndex,
        //    [Argument(LongName = "file-ouput", ShortName = 'o', Help = "Output file path")]
        //    string fileOutput = null
        //)
        //{
        //    //filesIndex = new List<string>()
        //    //{
        //    //    "Sample/Index2.md",
        //    //    "Sample/Index.md"
        //    //};

        //    var content = "";
        //    var anchors = new List<Anchor>();
        //    var compilers = new List<Compiler>();

        //    // compile all files
        //    foreach (var file in filesIndex)
        //    {
        //        var compiler = new Compiler(anchors, file);
        //        compiler.Compile();
        //        compilers.Add(compiler);
        //    }

        //    // Process all string parts ordered by priority
        //    var stringParts = compilers.SelectMany(f => f.GetStringParts());
        //    Compiler.Process(stringParts);

        //    // generate output
        //    foreach (var compiler in compilers)
        //    {
        //        content += compiler.GetContent();
        //        if (compiler != compilers.LastOrDefault())
        //            content += "\r\n";
        //    }

        //    if (fileOutput != null)
        //    {
        //        FileHelper.SaveContentToFile(content, fileOutput);
        //        return null;
        //    }

        //    return content;
        //}

        public void Main(
            [Argument(LongName = "config", ShortName = 'c', Help = "Config file")]
            string config = "Sample/Sample.xml"
        )
        {
            App.Console.Write(Translation.Translator.Translate(config, "pt-br", "en-us"));
            //var doc = new Documentation(config);
            //doc.Save();
        }
    }
}