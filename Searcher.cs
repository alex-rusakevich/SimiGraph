﻿using Scriban.Runtime;
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
        HttpClient client = new HttpClient(new HttpClientHandler
        {
            UseProxy = false
        });
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

        List<string> hanziList = new List<string>();
        string graphemes;

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

        public void RunAndGetResultList()
        {
            List<FoundObj> resultList = new List<FoundObj>();

            using (var countdownEvent = new CountdownEvent(hanziList.Count))
            {
                for (var i = 0; i < hanziList.Count; i++)
                    ThreadPool.QueueUserWorkItem(
                        async x =>
                        {
                            resultList.AddRange(await HanziToFoundObjList((string)x!));
                            countdownEvent.Signal();
                        }, this.hanziList[i]);

                countdownEvent.Wait();
            }

            resultList = resultList.DistinctBy(foundObj => foundObj.ToString()).ToList();
            resultList.RemoveAll(item => item == null);

            Directory.CreateDirectory("Results");

            using (StreamWriter writerForText = new StreamWriter(@$"Results\{this.graphemes}.html")) // Specialized result folders
            {
                TemplateContext context = new TemplateContext();
                context.LoopLimit = 0;

                var scriptObject = new ScriptObject();
                scriptObject.Add("find_obj_list", resultList);
                scriptObject.Add("graphemes", this.graphemes);
                context.PushGlobal(scriptObject);

                var template = Template.ParseLiquid(Properties.Resources.ResultTemplate);
                string templateStr = template.Render(context);

                writerForText.Write(templateStr);
            }

            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@$"Results\{this.graphemes}.html")
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}
