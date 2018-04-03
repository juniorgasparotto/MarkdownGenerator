namespace MarkdownGenerator.Xml
{
    public class Language
    {
        public string Name { get; set; }
        public string Output { get; set; }
        public bool IsDefault { get; set; }
        public string UrlBase { get; internal set; }
    }
}
