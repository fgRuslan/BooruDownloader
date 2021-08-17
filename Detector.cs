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
            => engines.First(x => x.chkHost(url)).GenEngine();
    }
}
