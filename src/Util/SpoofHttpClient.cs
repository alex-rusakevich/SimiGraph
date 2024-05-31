namespace SimiGraph.Util
{
    /// <summary>
    /// <para>Auto-set user-like headers in HttpClient requests</para>
    /// </summary>
    public class SpoofHttpClient : HttpClient
    {
        private static SpoofHttpClient? _instance;

        public static SpoofHttpClient Instance
        {
            get
            {
                _instance ??= new SpoofHttpClient();
                return _instance;
            }
        }

        private static readonly Dictionary<string, string> headers = new()
        {
            ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
            ["Accept-Language"] = "en-US,en;q=0.9,it;q=0.8,es;q=0.7",
            ["Accept-Encoding"] = "identity",
            ["Referer"] = "https://google.com/",
        };

        private SpoofHttpClient() : base()
        {
            foreach (KeyValuePair<string, string> item in headers)
            {
                DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            BaseAddress = new Uri("https://www.zdic.net");
        }
    }
}
