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
    public class Compiler
    {
        private const string IgnoreIncludeLabel = "@ignore";
        private const string OnlyWordPattern = "[a-zA-Z0-9_-]+";
        private static string IncludePattern = $@"^(#include\s""(.+?)"")(\s*|\s+{IgnoreIncludeLabel}\s*)$";
        private static string XmlMethodPattern = $@"(<{OnlyWordPattern}(?:\s.+?\s*)?/>)";

        private List<StringPart> parts = new List<StringPart>();
        private string file;

        private List<Header> headers = new List<Header>();
        private List<Anchor> anchors;
        private List<XmlMethod> xmlMethods = new List<XmlMethod>();
       

        public Compiler(List<Anchor> anchors, string file)
        {
            this.anchors = anchors;
            this.file = file;
            xmlMethods.Add(new XmlMethod("anchor-set", AnchorSet, 0));
            xmlMethods.Add(new XmlMethod("header-set", HeaderSet, 0));
            xmlMethods.Add(new XmlMethod("anchor-get", AnchorGet, 1));
            xmlMethods.Add(new XmlMethod("table-of-contents", GetTableOfContents, 1));
        }

        public void Compile()
        {
            var content = IncludeAllFiles(this.file);
            CompileTags(content);
        }

        public IEnumerable<StringPart> GetStringParts()
        {
            return parts;
        }

        public string GetContent()
        {
            var strBuilder = new StringBuilder();
            foreach (var part in parts)
                strBuilder.Append(part.ToString());

            return strBuilder.ToString();
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

        private void CompileTags(string content)
        {
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
                    new XmlMethodPart(parts, str, node, xmlMethod);
                }
                else
                {
                    new RealStringPart(parts, str);
                }
            }
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

        public static void Process(IEnumerable<StringPart> parts)
        {
            var partsXml = parts.Where(f => f is XmlMethodPart).Cast<XmlMethodPart>();
            foreach (var part in partsXml.OrderBy(m => m.XmlMethod.Priority))
                part.Process();
        }

        #region Xml methods

        private string HeaderSet(XmlMethodPart part)
        {
            var before = part.GetBeforeString();

            if (before != null)
            {
                string headerPattern = $@"^(#+ )(.+)$";
                var leftContent = before.RealString;
                var leftLineUntilStart = GetUntilStartLine(leftContent.Length - 1, leftContent);
                var matchHeader = Regex.Match(leftLineUntilStart, headerPattern);

                if (matchHeader.Groups.Count == 3)
                {
                    var level  = matchHeader.Groups[1].Value.Trim();
                    var title = matchHeader.Groups[2].Value.Trim();
                    var anchorName = part.Node.Attributes["anchor-name"];

                    Anchor anchor = null;
                    if (anchorName != null)
                    {
                        anchor = new Anchor()
                        {
                            Name = anchorName.Value,
                            Text = title
                        };
                        anchors.Add(anchor);
                    }

                    var header = new Header();
                    header.Anchor = anchor;
                    header.Level = level;
                    header.Title = title;
                    headers.Add(header);

                    var anchorHtml = "";
                    if (anchor != null)
                        anchorHtml = GetAnchor(anchor.Name, null);

                    before.NewString = leftContent.Remove(leftContent.Length - leftLineUntilStart.Length);
                    return header.Level + " " + anchorHtml + header.Title;
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
                    Text = anchorTextAttr?.Value
                };
                anchors.Add(anchor);
                return GetAnchor(anchor.Name, anchor.Text);
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

                var lastLines = "";
                var i = 0;
                var before = (StringPart)part;
                while (before != null && i < 10)
                {
                    i++;
                    lastLines = before.RealString + lastLines;
                    before = before.Before;
                }
                throw new Exception($"The anchor '{anchorNameAttr.Value}' doesn't exists at line: {lastLines}");
            }

            throw new Exception($"The tag <anchor-get> needs the attribute 'name'");
        }

        private string GetTableOfContents(XmlMethodPart part)
        {
            var strBuilder = new StringBuilder();
            foreach (var header in headers)
            {
                if (header.Level.Length > 1)
                    strBuilder.Append(new String(' ', header.Level.Length));

                strBuilder.Append("*");
                strBuilder.Append($" " + GetLinkFromAnchor(header.Title, header.Anchor.Name));
                if (header != headers.LastOrDefault())
                    strBuilder.AppendLine();
            }
            return strBuilder.ToString();
        }

        #endregion

        #region MarkDown methods

        private string GetLinkFromAnchor(string text, string anchor)
        {
            return $"[{text ?? anchor}](#{anchor})";
        }

        private string GetAnchor(string name, string text)
        {
            return $@"<a name=""{name}""></a>{text}";
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