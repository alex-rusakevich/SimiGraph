namespace SimiGraph.FileFormat
{
    public interface IFileFormatProcessor
    {
        abstract MemoryStream GenerateFormattedResult(List<FoundObj> resultList, string graphemes, string filepath);
    }
}
