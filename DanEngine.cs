using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace BooruDownloader
{
    class DanEngine : EngineBase
    {
        public override type getType()
        {
            return type.DAN;
        }

        public override string ExtensionFromUrl(string line)
        {
            var extension = "";
            var match = Regex.Match(line, "(?:)\\.[\\d\\w]+$", RegexOptions.Compiled);
            if (match.Success)
                extension = match.Value;
            return extension;
        }
        public override string FilenameFromUrl(string line)
        {
            var filename = "";
            var match = Regex.Match(line, "(?:)[\\d\\w]+\\.[\\d\\w]+$", RegexOptions.Compiled);
            if (match.Success)
                filename = match.Value;
            return filename;
        }

        public override string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        public override async void DownloadImage(string url, string tags, bool keepOriginalNames, string rating)
        {
            //I don't know what is this shit!
            string fullpath = "./out/" + rating + tags + ExtensionFromUrl(url);
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            fullpath = r.Replace(fullpath, "");

            string shortPath = Path.GetFullPath("./out/" + rating + ExtensionFromUrl(url));
            string extension = fullpath.Substring(fullpath.Length - 5);
            if (fullpath.Length > 259)
                fullpath = Truncate(fullpath, 259 - shortPath.Length - 4);
            fullpath = fullpath.Substring(5);

            using (WebClient wc = new WebClient())
            {
                // wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                //                      "Windows NT 5.2; .NET CLR 1.0.3705;)");
                wc.Headers.Add("user-agent", ".NET Framework Test Client");
                try
                {
                    if (keepOriginalNames)
                        wc.DownloadFileTaskAsync(new System.Uri(url), "./out/" + rating + FilenameFromUrl(url)).Wait();
                    else
                        wc.DownloadFileTaskAsync(new System.Uri(url), "./out/" + fullpath + extension).Wait();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ex.GetType().ToString());
                    //wc.DownloadFileAsync(new System.Uri(url), "./out/" + rating + FilenameFromUrl(url));
                }
            }
        }

        public override string DownloadPosts(string domain, string tags, int page, bool keepOriginalNames, bool includeRating)
        {
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/posts.xml?page=dapi&s=post&q=index&limit=1&tags=" + tags + "&page=" + page + "&api_key=" + this.apiKey + "&login=" + this.login);
            request.UserAgent = ".NET Framework Test Client";
            request.Accept = "text/xml";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string responseString = reader.ReadToEnd();

                document.LoadXml(responseString);
                root = document.DocumentElement;
                XmlNode node = document.DocumentElement;
                XmlNode sourceNode = node.SelectSingleNode("post/file-url");
                XmlNode tagsNode = node.SelectSingleNode("post/tag-string");
                XmlNode ratingNode = node.SelectSingleNode("post/rating");

                if (sourceNode == null)
                {
                    return "";
                }

                url = sourceNode.InnerXml;
                var postTags = tagsNode.InnerXml;
                var rating = ratingNode.InnerXml;

                if (includeRating)
                {
                    if (rating == "q")
                        rating = "questionable ";
                    if (rating == "e" || rating == "explicit")
                        rating = "nsfw ";
                    if (rating == "s")
                        rating = "safe ";
                }
                Console.WriteLine(url);
                DownloadImage(url, postTags, keepOriginalNames, rating);
            }
            return url;
        }

        public override int GetPostCount(string domain, string tags)
        {
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            string requestString = domain + "/posts.xml?page=dapi&s=post&q=index&tags=" + tags;

            if (this.login != "" && this.apiKey != "")
                requestString += "&api_key=" + this.apiKey + "&login=" + this.login;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
            request.UserAgent = ".NET Framework Test Client";
            request.Accept = "text/xml";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
#if DEBUG
            AllocConsole();
#endif
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string responseString = reader.ReadToEnd();
                document.LoadXml(responseString);
                root = document.DocumentElement;

                XmlNodeList elementList = root.GetElementsByTagName("post");
                return Convert.ToInt32(elementList.Count);
            }
        }
    }
}