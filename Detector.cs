using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooruDownloader
{
    public abstract class engineBase
    {
        List<string> hostsContainer = new List<string>();
        public string host
        {
            get
            {
                return hostsContainer[0];
            }
            set
            {
                hostsContainer.Add(value);
            }
        }
        public abstract EngineBase GenEngine();

        public bool CheckHost(string host)
        {
            return hostsContainer.Contains(host);
        }
    }

    public class danEngine : engineBase
    {
        public danEngine()
        {
            host = "danbooru.donmai.us";
            host = "safebooru.donmai.us";
        }

        public override EngineBase GenEngine()
        {
            return new DanEngine();
        }
    }
    public class gelEngine : engineBase
    {
        public gelEngine()
        {
            host = "rule34.xxx";
            host = "safebooru.org";
            host = "gelbooru.com";
        }

        public override EngineBase GenEngine()
        {
            return new GelEngine();
        }
    }
    class Detector
    {
        public static engineBase[] engines = new engineBase[] {
            new gelEngine(), new danEngine()
        };
        public static EngineBase detectEngine(String url)
        {
            try
            {
                return engines.First(x => x.CheckHost(GetHost(url))).GenEngine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        private static string GetHost(string uri)
        {
            for (int idx = 0; idx < uri.Length; idx++)
                if (char.Equals(uri[idx], '/') && char.Equals(uri[idx + 1], '/'))
                    return LoopUntilSlash(uri, idx + 2);
            return LoopUntilSlash(uri, 0);
        }

        private static string LoopUntilSlash(string l, int idx)
        {
            StringBuilder builder = new StringBuilder();
            builder.Capacity = l.Length;
            for (int i = idx; i < l.Length; i++)
            {
                if (!char.Equals(l[i], '/'))
                    builder.Append(l[i]);
                else
                    return builder.ToString();
            }
            return builder.ToString();
        }
    }
}