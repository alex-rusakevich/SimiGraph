using Scriban.Runtime;
using Scriban;
using System.Diagnostics;

namespace SimiGraph
{
    static class FileFormatProcessor
    {
        public static string savefileFilter = 
            "Веб-сайт (*.html)|*.html|Текстовый файл (*.txt)|*.txt|Все файлы (*.*)|*.*";

        static void SaveAsHTML(List<FoundObj> resultList, string graphemes, string filepath)
        {
            TemplateContext context = new TemplateContext();
            context.LoopLimit = 0;

            var scriptObject = new ScriptObject();
            scriptObject.Add("find_obj_list", resultList);
            scriptObject.Add("graphemes", graphemes);
            context.PushGlobal(scriptObject);

            var template = Template.ParseLiquid(Properties.Resources.ResultTemplate);
            string templateStr = template.Render(context);

            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.Write(templateStr);
            }
        }

        static void SaveAsTxt(List<FoundObj> resultList, string graphemes, string filepath)
        {
            int pairNum = resultList.Count;

            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.WriteLine($"Сравнительный список для графем(ы) {graphemes} ({pairNum} пар(ы))\n");

                int counter = 1;

                foreach (var item in resultList)
                {
                    sw.WriteLine($"{counter++}. {item.comp1} ~ {item.comp2}");
                }
            }
        }

        public static void SaveResultAs(List<FoundObj> resultList, string graphemes, string filepath)
        {
            switch (Path.GetExtension(filepath.ToLower()).ToLower())
            {
                case ".html":
                    SaveAsHTML(resultList, graphemes, filepath);
                    break;
                case ".txt":
                    SaveAsTxt(resultList, graphemes, filepath);
                    break;
                default:
                    SaveAsTxt(resultList, graphemes, filepath);
                    break;
            }

            var p = new Process();
            p.StartInfo = new ProcessStartInfo(filepath)
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}
