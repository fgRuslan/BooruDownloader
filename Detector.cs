using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooruDownloader
{
    public abstract class engineBase
    {
        List<string> hostsContainer = new List<string>();
        public string host { get { return hostsContainer[0]; } set { hostsContainer.Add(value); }  }
        public abstract EngineBase GenEngine();

        public bool chkHost(string host)
            => hostsContainer.Contains(host);
    }

    public class danEngine : engineBase
    {
        public danEngine() { host = "danbooru.donmai.us";  host = "danbooru.donmai.us"; }

        public override EngineBase GenEngine()
            => new DanEngine();
    }
    public class gelEngine : engineBase
    {
        public gelEngine() { host = "rule34.xxx"; host = "safebooru.org"; host = "gelbooru.org"; }

        public override EngineBase GenEngine()
            => new GelEngine();
    }
    class Detector
    {
        public static engineBase[] engines = new engineBase[] { new gelEngine(), new danEngine() };
        public static EngineBase detectEngine(String url)
            => engines.First(x => x.chkHost(gHost(url))).GenEngine();
        
        private string gHost(string uri)
        {
            for (int idx = 0; idx < uri.Length; idx++)
                if (char.Equals(uri[idx], '/') && char.Equals(uri[idx + 1], '/'))
                    return LoopUntilSlash(uri, idx + 2);
            return LoopUntilSlash(uri, 0);
        }

        private string LoopUntilSlash(string l, int idx)
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
