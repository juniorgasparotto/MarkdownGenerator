namespace MarkdownMerge.Commands
{
    public class Header
    {
        public string Title { get; set; }
        public Anchor Anchor { get; set; }
        public string Level { get; internal set; }
    }
}
