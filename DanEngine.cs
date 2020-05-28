using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Forms;

namespace BooruDownloader
{
    class DanEngine
    {
        XmlDocument doc = new XmlDocument();
        XmlElement root;
        string url;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        string ExtFromURL(string line)
        {
            var ext = "";
            var match = Regex.Match(line, "(?:)\\.[\\d\\w]+$", RegexOptions.Compiled);
            if (match.Success)
                ext = match.Value;
            return ext;
        }
        string FnameFromURL(string line)
        {
            var fname = "";
            var match = Regex.Match(line, "(?:)[\\d\\w]+\\.[\\d\\w]+$", RegexOptions.Compiled);
            if (match.Success)
                fname = match.Value;
            return fname;
        }

        string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        private async void downloadImage(string url, string tags, bool keepOriginalNames, string rating)
        {
            //I don't know what is this shit!
            string fullpath = "./out/" + rating + tags + ExtFromURL(url);
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            fullpath = r.Replace(fullpath, "");

            string shortPath = Path.GetFullPath("./out/" + rating + ExtFromURL(url));
            string extension = fullpath.Substring(fullpath.Length - 5);
            if (fullpath.Length > 259)
                fullpath = Truncate(fullpath, 259 - shortPath.Length - 4);
            fullpath = fullpath.Substring(5);

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                      "Windows NT 5.2; .NET CLR 1.0.3705;)");
                try
                {
                    if (keepOriginalNames)
                        wc.DownloadFileAsync(new System.Uri(url), "./out/" + rating + FnameFromURL(url));
                    else
                        await wc.DownloadFileTaskAsync(new System.Uri(url), "./out/" + fullpath + extension);
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show(ex.ToString(), ex.GetType().ToString());
#endif
                    MessageBox.Show(ex.ToString(), ex.GetType().ToString());
                    //wc.DownloadFileAsync(new System.Uri(url), "./out/" + rating + FnameFromURL(url));
                }
            }
        }

        public string downloadPosts(string domain, string tags, int page, bool keepOriginalNames, bool includeRating)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/posts.xml?page=dapi&s=post&q=index&limit=1&tags=" + tags + "&page=" + page);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();

                doc.LoadXml(responseString);
                root = doc.DocumentElement;
                XmlNode node = (XmlNode)doc.DocumentElement;
                XmlNode sourceNode = node.SelectSingleNode("post/file-url");
                XmlNode tagsNode = node.SelectSingleNode("post/tag-string");
                XmlNode ratingNode = node.SelectSingleNode("post/rating");
                if (sourceNode == null)
                {
                    return "";
                }
                url = sourceNode.InnerXml;
                var tagstring = tagsNode.InnerXml;
                var rating = ratingNode.InnerXml;
                var ratingstr = "";
                if (includeRating)
                {
                    if (rating == "q")
                        ratingstr = "questionable ";
                    if (rating == "e")
                        ratingstr = "nsfw ";
                    if (rating == "s")
                        ratingstr = "safe ";
                }
                Console.WriteLine(url);
                downloadImage(url, tagstring, keepOriginalNames, ratingstr);
            }
            return url;
        }

        public int getPostCount(string domain, string tags)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/posts.xml?page=dapi&s=post&q=index&tags=" + tags);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
#if DEBUG
            AllocConsole();
#endif
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();
                doc.LoadXml(responseString);
                root = doc.DocumentElement;
                
                XmlNodeList elemList = root.GetElementsByTagName("post");
                int count = 0;
                for (int i = 0; i < elemList.Count; i++)
                {
                    count++;
                }
                return Convert.ToInt32(count);
            }
        }
    }
}
