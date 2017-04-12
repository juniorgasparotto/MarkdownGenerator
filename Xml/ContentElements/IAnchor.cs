namespace MarkdownMerge.Xml.Content
{
    internal interface IAnchor
    {
        string Name { get; set; }
        string Text { get;  set; }
        string GetAnchorLink();
    }
}