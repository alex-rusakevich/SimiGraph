using Scriban.Runtime;
using Scriban;
using System.Text;

namespace SimiGraph.FileFormat.Processors
{
    public class HtmlProcessor: IFileFormatProcessor
    {
        public MemoryStream GenerateFormattedResult(List<FoundObj> resultList, string graphemes, string filepath)
        {
            TemplateContext context = new()
            {
                LoopLimit = 0
            };

            var scriptObject = new ScriptObject
            {
                { "find_obj_list", resultList },
                { "graphemes", graphemes }
            };
            context.PushGlobal(scriptObject);

            var template = Template.ParseLiquid(Properties.Resources.ResultTemplate);
            return new MemoryStream(
                Encoding.UTF8.GetBytes(template.Render(context)));
        }
    }
}
