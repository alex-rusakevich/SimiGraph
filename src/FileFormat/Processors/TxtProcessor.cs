namespace SimiGraph.FileFormat.Processors
{
    public class TxtProcessor : IFileFormatProcessor
    {
        public MemoryStream GenerateFormattedResult(List<FoundObj> resultList, string graphemes, string filepath)
        {
            var memStream = new MemoryStream();
            int pairNum = resultList.Count;

            using (StreamWriter sw = new(memStream, leaveOpen: true))
            {
                sw.WriteLine($"Сравнительный список для графем(ы) {graphemes} ({pairNum} пар(ы))\n");
                sw.WriteLine($"Примечание: текст ниже можно скопировать в Word и выбрать \"Преобразовать в таблицу\"\n");

                int counter = 1;

                foreach (var item in resultList)
                {
                    sw.WriteLine($"{counter++}.\t{item.comp1}\t{item.comp2}");
                }
            }

            return memStream;
        }
    }
}
