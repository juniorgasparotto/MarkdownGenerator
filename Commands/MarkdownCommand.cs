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

        public void ConvertMdFileToHtml(string baseDir)
        {
            //var path2 = @"D:\Junior\Projetos\GITHUB.COM\juniorgasparotto\SysCommand\doc\pt-br\extras\extras.md";
            //var str = FileHelper.GetContentFromFile(path2);
            //var fileBaseDir = Path.GetDirectoryName(path2).Replace("pt-br", "pt-br2");
            //var fileName = Path.GetFileNameWithoutExtension(path2);
            //var htmlFile = Path.Combine(fileBaseDir, fileName + ".md");
            //var html = Markdown.ToHtml(str);
            //var converter = new Converter();
            //string result = converter.Convert(html);
            //FileHelper.SaveContentToFile(result, htmlFile);
            //return;
            //var file = @"D:\Junior\Projetos\GITHUB.COM\juniorgasparotto\SysCommand\doc\pt-br\input\support-types.html";
            //var fileTraduzido = @"D:\Junior\Projetos\GITHUB.COM\juniorgasparotto\SysCommand\doc\pt-br\input\support-types-traduzido.html";
            //var fileMk = @"D:\Junior\Projetos\GITHUB.COM\juniorgasparotto\SysCommand\doc\pt-br\input\support-types-markdown.html";
            ////var html2 = Translation.Translator.Translate(File.ReadAllText(file), "pt-br", "en-us");
            //var html2 = File.ReadAllText(file);
            //FileHelper.SaveContentToFile(html2, fileTraduzido);
            //var converter2 = new Converter();
            //var d = converter2.Convert(html2);
            //FileHelper.SaveContentToFile(d, fileMk);
            //return;
            DirectoryHelper.FindFiles(baseDir, path =>
            {
                var str = FileHelper.GetContentFromFile(path);
                var fileBaseDir = Path.GetDirectoryName(path).Replace("pt-br", "pt-br2");
                var fileName = Path.GetFileNameWithoutExtension(path);
                var htmlFile = Path.Combine(fileBaseDir, fileName + ".md");
                var html = Markdown.ToHtml(str);
                var converter = new Converter();
                string result = converter.Convert(html);
                FileHelper.SaveContentToFile(result, htmlFile);
            }, "*.md");
        }

        public void Main(
            [Argument(LongName = "config", ShortName = 'c', Help = "Config file")]
            string config = "Sample/Sample.xml"
        )
        {
            //App.Console.Write(Translation.Translator.Translate(config, "pt-br", "en-us"));
            var doc = new Documentation(config);
            doc.Save();
        }
    }
}