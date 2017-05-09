using SysCommand.ConsoleApp;
using SysCommand.ConsoleApp.Helpers;
using MarkdownGenerator.Helpers;
using System.IO;
using Markdig;
using Html2Markdown;
using System.Threading;

namespace MarkdownGenerator.Commands
{
    public partial class WaitAttachDebugCommand : Command
    {
        public void WaitAttachDebug(int time = 20000)
        {
            App.Console.Write($"Waiting attach debug until {time} seconds");
            Thread.Sleep(time);
        }
    }
}