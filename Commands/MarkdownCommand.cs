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
    public class MarkdownCommand : Command
    {
        private const string IgnoreIncludeLabel = "@ignore";
        private const string OnlyWordPattern = "[a-zA-Z0-9_-]+";
        private static string IncludePattern = $@"^(#include\s""(.+?)"")(\s*|\s+{IgnoreIncludeLabel}\s*)$";
        private static string XmlMethodPattern = $@"(<{OnlyWordPattern}\s.+?\s*/>)";

        private List<Header> headers = new List<Header>();
        private List<Anchor> anchors = new List<Anchor>();

        private List<XmlMethod> xmlMethods = new List<XmlMethod>();
        
        public MarkdownCommand()
        {
            xmlMethods.Add(new XmlMethod("anchor-set", AnchorSet, 0));
            xmlMethods.Add(new XmlMethod("header-set", HeaderSet, 0));
            xmlMethods.Add(new XmlMethod("anchor-get", AnchorGet, 1));
        }

        public string Main(
            [Argument(LongName = "file-index", ShortName = 'i', Help = "Markdown file that represents the index of your documents")]
            string fileIndex = "Sample/Index.md",
            [Argument(LongName = "file-ouput", ShortName = 'o', Help = "Output file path")]
            string fileOutput = null,
            [Argument(LongName = "table-of-contents-place-holder", ShortName = 'p', Help = "Represents the table of contents. Use the text place holder where you want to replace to the real table of contents")]
            string tableOfContentsPlaceHolder = "____TABLE_OF_CONTENTS____"
        )
        {
            var content = IncludeAllFiles(fileIndex);
            content = CompileTags(content);

            var tableOfContents = CreateTableOfContents();
            if (!string.IsNullOrEmpty(tableOfContents))
            {
                if (string.IsNullOrWhiteSpace(tableOfContentsPlaceHolder))
                    content = tableOfContents + "\r\n" + content;
                else
                    content = content.Replace(tableOfContentsPlaceHolder, tableOfContents);
            }
            return content;
        }

        private string IncludeAllFiles(string fileName)
        {
            var content = FileHelper.GetContentFromFile(fileName);
            if (content == null)
                throw new Exception("File not found: " + fileName);

            content = Regex.Replace(content, IncludePattern, f => this.ReplaceIncludeByFile(f), RegexOptions.Multiline);
            return content;
        }

        private string ReplaceIncludeByFile(Match match)
        {
            if (match.Groups.Count == 4 && match.Groups[3].Value.Trim() == IgnoreIncludeLabel)
                return match.Groups[1].Value;

            var content = FileHelper.GetContentFromFile(match.Groups[2].Value);
            return content;
        }

        private string CompileTags(string content)
        {
            var strBuilder = new StringBuilder();
            var parts = new List<StringPart>();
            var partsXmls = new List<XmlMethodPart>();
            var split = Regex.Split(content, XmlMethodPattern);

            foreach (var str in split)
            {
                XmlNode node = null;
                XmlMethod xmlMethod = null;

                if (Regex.IsMatch(str, XmlMethodPattern))
                {
                    node = this.GetXmlNode(str);
                    xmlMethod = this.GetXmlMethod(node);
                }

                if (xmlMethod != null)
                {
                    var part = new XmlMethodPart(parts, str, node, xmlMethod);
                    partsXmls.Add(part);
                }
                else
                {
                    new RealStringPart(parts, str);
                }
            }

            foreach (var part in partsXmls.OrderBy(m => m.XmlMethod.Priority))
                part.Process();

            foreach (var part in parts)
                strBuilder.Append(part.ToString());

            content = strBuilder.ToString();
            return content;
        }

        private XmlNode GetXmlNode(string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);

            if (doc.ChildNodes.Count > 0)
                return doc.ChildNodes.Item(0);
            return null;
        }

        private XmlMethod GetXmlMethod(XmlNode node)
        {
            if (node != null)
            {
                var xmlMethod = xmlMethods.FirstOrDefault(f => f.Name == node.Name);
                if (xmlMethod != null)
                    return xmlMethod;
            }

            return null;
        }

        #region Xml methods

        private string HeaderSet(XmlMethodPart part)
        {
            var leftLine = part.GetBeforeString();

            if (leftLine != null)
            {
                string headerPattern = $@"^(#+ )(.+)$";
                var leftLineUntilStart = GetUntilStartLine(leftLine.Length - 1, leftLine);
                var matchHeader = Regex.Match(leftLineUntilStart, headerPattern);

                if (matchHeader.Groups.Count == 3)
                {
                    var anchorName = part.Node.Attributes["anchor-name"];
                    Anchor anchor = null;
                    if (anchorName != null)
                    {
                        anchor = new Anchor()
                        {
                            Name = anchorName.Value
                        };
                        anchors.Add(anchor);
                    }

                    var header = new Header();
                    header.Anchor = anchor;
                    header.Level = matchHeader.Groups[1].Value.Trim();
                    header.Name = matchHeader.Groups[2].Value.Trim();
                    headers.Add(header);

                    var anchorHtml = "";
                    if (anchor != null)
                        anchorHtml = GetAnchor(anchor.Name);

                    return header.Level + " " + header.Name + anchorHtml;
                }
            }

            return part.RealString;
        }

        private string AnchorSet(XmlMethodPart part)
        {
            var anchorNameAttr = part.Node.Attributes["name"];
            if (anchorNameAttr != null)
            {
                var anchorTextAttr = part.Node.Attributes["text"];
                Anchor anchor = new Anchor
                {
                    Name = anchorNameAttr.Value,
                    Text = anchorTextAttr?.Name
                };
                anchors.Add(anchor);
                return GetAnchor(anchor.Name);
            }

            return "";
        }

        private string AnchorGet(XmlMethodPart part)
        {
            var anchorNameAttr = part.Node.Attributes["name"];
            if (anchorNameAttr != null)
            {
                var anchor = anchors.FirstOrDefault(f => f.Name == anchorNameAttr.Value);
                if (anchor != null)
                    return GetLinkFromAnchor(anchor.Text, anchor.Name);
            }

            return "";
        }

        #endregion

        #region MarkDown methods

        private string GetLinkFromAnchor(string text, string anchor)
        {
            return $"[{text ?? anchor}](#{anchor})";
        }

        private string GetAnchor(string name)
        {
            return $@"<a name=""{name}""></a>";
        }

        private string CreateTableOfContents()
        {
            var strBuilder = new StringBuilder();
            foreach (var header in headers)
            {
                if (header.Level.Length > 1)
                    strBuilder.Append(new String(' ', header.Level.Length));

                strBuilder.Append("*");
                strBuilder.Append($" " + GetLinkFromAnchor(header.Name, header.Anchor.Name));
                if (header != headers.LastOrDefault())
                    strBuilder.AppendLine();
            }
            return strBuilder.ToString();
        }

        #endregion

        #region String navigation methods

        private string GetUntilStartLine(int endPos, string content)
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

        private string GetRightUntilEndLine(int startPos, string content)
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

        #endregion
    }
}
