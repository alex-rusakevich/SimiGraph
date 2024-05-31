using System.Text.RegularExpressions;
using System.Web;
using SimiGraph.Util;


namespace SimiGraph
{
    public class FoundObj
    {
        public string? Comp1 { get; set; }
        public string? Comp2 { get; set; }
        public override string ToString()
        {
            return $"{Comp1} ~ {Comp2}";
        }
    }

    internal partial class Searcher
    {
        private readonly SpoofHttpClient client = SpoofHttpClient.Instance;
        private readonly List<string> hanziList = [];
        private readonly string graphemes;

        /// <summary>
        /// Find all values between 同“ and “ and return them as a list.
        /// </summary>
        /// <param name="txt">Text to search in, should be zdic page's html</param>
        /// <returns>List of values present in 同“”</returns>
        public static List<string> StrToListOfComp2(string txt)
        {
            List<string> result = [];
            var matches = TongRegex().Matches(txt);

            foreach (Match match in matches)
            {
                result.Add(match.Groups[1].Value);
            }

            return result;
        }

        public Searcher(string graphemes)
        {
            if(string.IsNullOrWhiteSpace(graphemes))
            {
                throw new ArgumentException("graphemes must not contain empty string or null");
            }

            this.graphemes = graphemes;

            foreach(var grapheme in graphemes)
            {
                SetHanziListByGrapheme(grapheme);
            }

            this.hanziList = this.hanziList.Distinct().ToList();
        }

        public async Task<List<FoundObj>> HanziToFoundObjList(string hanzi)
        {
            List<FoundObj> resultList = [];
                
            var encodedHanzi = HttpUtility.UrlEncode(hanzi);
            string link = $"hans/{encodedHanzi}";
            string htmlCode = await client.GetStringAsync(link).ConfigureAwait(false);

            htmlCode = HtmlTagRegex().Replace(htmlCode, String.Empty);
            htmlCode = htmlCode.Replace("&ldquo;", "“");
            htmlCode = htmlCode.Replace("&rdquo;", "”");
            var comp2List = StrToListOfComp2(htmlCode);

            foreach (var comp2 in comp2List)
            {
                if(hanzi != comp2)
                {
                    var noDigitsComp2 = NumberRegex().Replace(comp2, "");

                    resultList.Add(new FoundObj()
                    {
                        Comp1 = hanzi,
                        Comp2 = noDigitsComp2
                    });
                }
            }

            return resultList;
        }

        /// <summary>
        /// Fetch all hanzi which have grapheme and add them to this.hanziList.
        /// <example>
        /// <code>
        /// SetHanziListByGrapheme('贝'); // this.hanziList == {'呗', ...}
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="grapheme"></param>
        public void SetHanziListByGrapheme(char grapheme)
        {
            List<string> hanziList = [];

            string graphemeWebsite = client.GetStringAsync(
                "https://www.zdic.net/zd/bs/bs/?bs=" 
                + HttpUtility.UrlEncode(grapheme.ToString())).Result;

            foreach (Match match in HanziInGraphemeHtmlRegex().Matches(graphemeWebsite))
            {
                var hanzi = match.Groups[1].Value.Trim();
                hanziList.Add(hanzi.Trim());
            }

            this.hanziList.AddRange(hanziList);
        }

        /// <summary>
        /// Generate and return list with results
        /// </summary>
        /// <returns>Key is graphemes, value is List<FoundObj></returns>
        public KeyValuePair<string, List<FoundObj>> GetGraphemesAndResultList()
        {
            List<FoundObj> resultList = [];
            List<Task> hanziToListTasks = [];

            #region Fetching and processing info about all hanzi
            foreach (var hanzi in hanziList)
            {
                Task hanziTask = Task.Run(async () =>
                {
                    resultList.AddRange(await HanziToFoundObjList(hanzi));
                });

                hanziToListTasks.Add(hanziTask);
            }

            Task.WaitAll([..hanziToListTasks]);
            #endregion

            resultList.RemoveAll(item => item == null);
            resultList = resultList.DistinctBy(foundObj => foundObj.ToString()).ToList();

            return new KeyValuePair<string, List<FoundObj>>(this.graphemes, resultList);
        }

        [GeneratedRegex(@"同“([^“”]+)”")]
        private static partial Regex TongRegex();

        [GeneratedRegex("<[^>]*>")]
        private static partial Regex HtmlTagRegex();

        [GeneratedRegex(@"\d+")]
        private static partial Regex NumberRegex();

        [GeneratedRegex(@"<A[^>]*_blank>([^<]*)<\/A>")]
        private static partial Regex HanziInGraphemeHtmlRegex();
    }
}
