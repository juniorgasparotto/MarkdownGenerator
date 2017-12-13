using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownGenerator.Helpers
{
    public static class DirectoryHelper
    {
        public static void FindFiles(string dirPath, Action<string> foundCallBack, string searchPattern)
        {
            foreach (string f in Directory.GetFiles(dirPath, searchPattern))
                foundCallBack(f);

            foreach (string d in Directory.GetDirectories(dirPath))
                FindFiles(d, foundCallBack, searchPattern);
        }
    }
}
