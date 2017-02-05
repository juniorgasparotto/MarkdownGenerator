using SysCommand.ConsoleApp;
using SysCommand.ConsoleApp.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System;
using SysCommand.Mapping;
using System.Xml;
namespace MarkdownMerge.Commands
{
    public class RealStringPart : StringPart
    {
        public RealStringPart(List<StringPart> parts, string realString)
            : base(parts, realString)
        {
        }

        public override void Process()
        {
            
        }
    }
}
