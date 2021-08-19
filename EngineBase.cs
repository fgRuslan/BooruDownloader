using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BooruDownloader
{
    public abstract class EngineBase
    {
        public XmlDocument doc = new XmlDocument();
        public XmlElement root;
        public string url;
        public string rating;

        public enum type
        {
            DAN = 0,
            GEL = 1,
        };

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        public abstract string ExtFromURL(string line);
        public abstract string FnameFromURL(string line);
        public abstract string Truncate(string line, int mChar);
        public abstract void downloadImage(string url, string tags, bool keepOrigName, string rating);
        public abstract string downloadPosts(string url, string tags, int page, bool keepOrigName, bool inclRating);
        public abstract int getPostCount(string domain, string tags);
        public abstract type getType();
    }
}