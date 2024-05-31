using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimiGraph.FileFormat
{
    public interface IFileFormatProcessor
    {
        abstract MemoryStream GenerateFormattedResult(List<FoundObj> resultList, string graphemes, string filepath);
    }
}
