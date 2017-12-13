using System;
using System.Linq;

namespace MarkdownGenerator.Helpers
{
    public static class StringHelper
    {
        public static string GetUntilStartLine(int endPos, string content)
        {
            int startPos = 0;

            for(var i = endPos; i >= 0; i--)
            {
                var charPos = content[i];
                if (charPos == '\n' || charPos == '\r')
                {
                    startPos = i + 1;
                    break;
                }
            }

            return content.Substring(startPos, (endPos - startPos + 1));
        }

        public static string GetRightUntilEndLine(int startPos, string content)
        {
            int length = 0;
            for (var i = startPos; i < content.Length; i++)
            {
                if (content[i] != '\n' && content[i] != '\r')
                    length++;
                else
                    break;
            }
            return content.Substring(startPos, length);
        }

        public static Uri AppendUri(Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }

        public static string TrimAllLines(string text)
        {
            return string.Join("\n", text.Split('\n').Select(s => s.Trim()));
        }


        public static string GetAnchorLink(string urlBase, string outputPath, string anchorName)
        {
            var path = "";
            if (!string.IsNullOrEmpty(urlBase))
                path = AppendUri(new Uri(urlBase), outputPath).ToString();
            return path + "#" + anchorName;
        }
    }
}