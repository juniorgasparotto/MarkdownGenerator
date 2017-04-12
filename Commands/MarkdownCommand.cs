using SysCommand.ConsoleApp;
using SysCommand.ConsoleApp.Helpers;
using System.Collections.Generic;
using System.Linq;
using SysCommand.Mapping;
using MarkdownMerge.Xml.Content;
using MarkdownMerge.Xml;
using MarkdownMerge.Helpers;
using System.IO;
using Markdig;
using Html2Markdown;
using Markdig.Renderers;
using System;
using Markdig.Syntax;
using HtmlAgilityPack;
using System.Text;

namespace MarkdownMerge.Commands
{
    public partial class MarkdownCommand : Command
    {
        public MarkdownCommand()
        {
            this.HelpText = "Merge markdown files; Generate automatic table of contents; Generate anchor reference;";

            //HtmlNode.ElementsFlags["header-set"] = HtmlElementFlag.Empty;
            //HtmlNode.ElementsFlags["header-get"] = HtmlElementFlag.Empty;
            //HtmlNode.ElementsFlags["anchor-get"] = HtmlElementFlag.Empty | HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["a"] = HtmlElementFlag.Empty | HtmlElementFlag.Closed | HtmlElementFlag.CanOverlap;
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

        public void TestConvertMdFile(string filePath)
        {
            var path = filePath;
            var str = FileHelper.GetContentFromFile(path);
            //var fileBaseDir = Path.GetDirectoryName(path).Replace("pt-br", "pt-br2");
            var fileBaseDir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var htmlFile = Path.Combine(fileBaseDir, fileName + ".test.md");
            var html = Markdown.ToHtml(str);
            var converter = new Converter();
            string result = converter.Convert(html);
            FileHelper.SaveContentToFile(result, htmlFile);
        }

        public void TestConvertMdFolder(string baseDir, string baseDirOutput)
        {
            if (baseDir != baseDirOutput)
            {
                DirectoryHelper.FindFiles(baseDir, path =>
                {
                    var str = FileHelper.GetContentFromFile(path);
                    //var fileBaseDir = Path.GetDirectoryName(path).Replace("pt-br", "pt-br2");
                    var fileBaseDir = baseDirOutput;
                    var fileName = Path.GetFileNameWithoutExtension(path);
                    var htmlFile = Path.Combine(fileBaseDir, fileName + ".md");
                    var html = Markdown.ToHtml(str);
                    var converter = new Converter();
                    string result = converter.Convert(html);
                    FileHelper.SaveContentToFile(result, htmlFile);
                }, "*.md");
            }
        }

        public void Main(
            [Argument(Help = "Base directory")]
            string baseDir = @"D:\Junior\Projetos\GITHUB.COM\juniorgasparotto\SysCommand",
            [Argument(Help = "Index file")]
            string indexFile = @"documentation\.generator\index.xml"
        )
        {
            baseDir = null;
            indexFile = "Sample/Sample.xml";

            if (baseDir != null)
                Directory.SetCurrentDirectory(baseDir);

            //App.Console.Write(Translation.Translator.Translate(config, "pt-br", "en-us"));
            var doc = new Documentation(indexFile);
            doc.Save();
        }
    }
}