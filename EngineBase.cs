using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;

namespace BooruDownloader
{
    public abstract class EngineBase
    {
        public XmlDocument document = new XmlDocument();
        public XmlElement root;
        public XmlElement post;
        public string url;
        public string rating;

        protected string apiKey;
        protected string login;

        public enum type
        {
            DAN = 0,
            GEL = 1,
        };

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        public abstract void DownloadImage(string url, string tags, bool keepOrigName, string rating);
        public abstract string DownloadPosts(string url, string tags, int page, bool keepOrigName, bool inclRating);
        public abstract int GetPostCount(string domain, string tags);
        public abstract type getType();

        public virtual void SetApiKey(string apiKey)
        {
            this.apiKey = apiKey;
        }
        public virtual void SetLogin(string login)
        {
            this.login = login;
        }

        public virtual string ExtensionFromUrl(string url)
        {
            var extension = "";
            var match = Regex.Match(url, @"(?<=\.)[^.]+$", RegexOptions.Compiled);
            if (match.Success)
                extension = match.Value;
            return extension;
        }

        public virtual string FilenameFromUrl(string url)
        {
            var filename = "";
            var match = Regex.Match(url, @"([^\/.]+)\.[^.]*$", RegexOptions.Compiled);
            if (match.Success)
                filename = match.Value;
            return filename;
        }
    }
}