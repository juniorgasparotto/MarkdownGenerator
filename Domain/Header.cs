using SysCommand.ConsoleApp;
using SysCommand.ConsoleApp.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MarkdownMerge.Commands
{
    public class Header
    {
        public string Title { get; set; }
        public Anchor Anchor { get; set; }
        public string Level { get; internal set; }
    }
}
