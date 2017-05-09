using SysCommand.ConsoleApp;
using SysCommand.ConsoleApp.Helpers;
using MarkdownGenerator.Helpers;
using System.IO;
using Markdig;
using Html2Markdown;

namespace MarkdownGenerator.Commands
{
    public partial class TestCommand : Command
    {
        public TestCommand()
        {
            this.OnlyInDebug = true;
            this.HelpText = "Tests features";
        }

        public void TestConvertMdFile(string filePath)
        {
            var path = filePath;
            var str = FileHelper.GetContentFromFile(path);
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
    }
}