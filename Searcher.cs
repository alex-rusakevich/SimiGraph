using Scriban.Runtime;
using Scriban;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;


namespace SimiGraph
{
    class FoundObj
    {
        public string? comp1 { get; set; }
        public string? comp2 { get; set; }
        public override string ToString()
        {
            return $"{comp1} ~ {comp2}";
        }
    }

    internal class Searcher
    {
        HttpClient client = new HttpClient();
        List<string> hanziList = new List<string>();
        string graphemes;

        /// <summary>
        /// Find all values between 同“ and “ and return them as a list.
        /// </summary>
        /// <param name="txt">Text to search in, should be zdic page's html</param>
        /// <returns>List of values present in 同“”</returns>
        public static List<string> StrToListOfComp2(string txt)
        {
            List<string> result = new List<string>();
            var matches = Regex.Matches(txt, @"同“([^“”]+)”");

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

            int MaxThreadsCount = Environment.ProcessorCount;
            ServicePointManager.DefaultConnectionLimit = MaxThreadsCount;

            client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,es;q=0.7");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "identity");
            client.DefaultRequestHeaders.Add("Referer", "https://google.com/");

            client.BaseAddress = new Uri("https://www.zdic.net");

            foreach(var grapheme in graphemes)
            {
                SetHanziListByGrapheme(grapheme);
            }

            this.hanziList = this.hanziList.Distinct().ToList();
        }

        public async Task<List<FoundObj>> HanziToFoundObjList(string hanzi)
        {
            List<FoundObj> resultList = new List<FoundObj>();
                
            var encodedHanzi = HttpUtility.UrlEncode(hanzi);
            string link = $"hans/{encodedHanzi}";
            string htmlCode = await client.GetStringAsync(link).ConfigureAwait(false);

            htmlCode = Regex.Replace(htmlCode, "<[^>]*>", String.Empty);
            htmlCode = htmlCode.Replace("&ldquo;", "“");
            htmlCode = htmlCode.Replace("&rdquo;", "”");
            var comp2List = StrToListOfComp2(htmlCode);

            foreach (var comp2 in comp2List)
            {
                if(hanzi != comp2)
                {
                    var noDigitsComp2 = Regex.Replace(comp2, @"\d+", "");

                    resultList.Add(new FoundObj()
                    {
                        comp1 = hanzi,
                        comp2 = noDigitsComp2
                    });
                }
            }

            return resultList;
        }

        public void SetHanziListByGrapheme(char grapheme)
        {
            List<string> hanziList = new List<string>();

            string graphemeWebsite = "";

            graphemeWebsite = client.GetStringAsync(
                "https://www.zdic.net/zd/bs/bs/?bs=" 
                + HttpUtility.UrlEncode(grapheme.ToString())).Result;

            foreach (Match match in Regex.Matches(graphemeWebsite, @"<A[^>]*_blank>([^<]*)<\/A>"))
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
            List<FoundObj> resultList = new List<FoundObj>();
            List<Task> hanziToListTasks = new List<Task>();

            #region Fetching and processing info about all hanzi
            foreach (var hanzi in hanziList)
            {
                Task hanziTask = Task.Run(async () =>
                {
                    resultList.AddRange(await HanziToFoundObjList(hanzi));
                });

                hanziToListTasks.Add(hanziTask);
            }

            Task.WaitAll(hanziToListTasks.ToArray());
            #endregion

            resultList = resultList.DistinctBy(foundObj => foundObj.ToString()).ToList();
            resultList.RemoveAll(item => item == null);

            return new KeyValuePair<string, List<FoundObj>>(this.graphemes, resultList);
        }
    }
}
