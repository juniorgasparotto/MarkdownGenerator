namespace MarkdownMerge.Xml.Tags
{
    internal interface IAnchor
    {
        string Name { get; set; }
        string Text { get;  set; }
        string GetAnchorLink();
    }
}