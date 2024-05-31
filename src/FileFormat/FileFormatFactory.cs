namespace SimiGraph.FileFormat
{
    public class FileFormatFactory
    {
        public static IFileFormatProcessor GetFormatProcessorByExt(string ext)
        {
            return ext switch
            {
                ".html" => new Processors.HtmlProcessor(),
                ".txt" => new Processors.TxtProcessor(),
                _ => new Processors.TxtProcessor(),
            };
        }
    }
}
