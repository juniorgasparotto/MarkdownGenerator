namespace MarkdownMerge.Helpers
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
    }
}