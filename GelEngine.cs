using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace BooruDownloader
{
    class GelEngine : EngineBase
    {
        public override type getType()
        {
            return type.GEL;
        }

        public override string ExtensionFromUrl(string line)
        {
            var ext = "";
            var match = Regex.Match(line, @"(?<=\.)[^.]+$", RegexOptions.Compiled);
            if (match.Success)
                ext = match.Value;
            return ext;
        }
        public override string FilenameFromUrl(string line)
        {
            var fname = "";
            var match = Regex.Match(line, @"([^\/.]+)\.[^.]*$", RegexOptions.Compiled);
            if (match.Success)
                fname = match.Value;
            return fname;
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
                //wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                //    "Windows NT 5.2; .NET CLR 1.0.3705;)");
                try
                {
                    if (keepOriginalNames)
                        wc.DownloadFileAsync(new System.Uri(url), "./out/" + rating + FilenameFromUrl(url));
                    else
                        await wc.DownloadFileTaskAsync(new System.Uri(url), "./out/" + fullpath + extension);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ex.GetType().ToString());
                    wc.DownloadFileAsync(new System.Uri(url), "./out/" + rating + FilenameFromUrl(url));
                }
            }

        }

        public override int GetPostCount(string domain, string tags)
        {
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tags + "&api_key=" + this.apiKey + "&user_id=" + this.login);
            request.UserAgent = ".NET Framework Test Client";
            request.Accept = "text/xml";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
#if DEBUG
            AllocConsole();
#endif
            string result = "";
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string responseString = reader.ReadToEnd();
                var match = Regex.Match(responseString, "count=\\\"(.+)\\\"", RegexOptions.Compiled);
                if (match.Success)
                {
                    Console.WriteLine(match.Groups[1].Value + " posts found");
                    result = match.Groups[1].Value;
                }
            }
            return Convert.ToInt32(result);
        }
        public override string DownloadPosts(string domain, string tags, int page, bool keepOriginalNames, bool includeRating)
        {
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tags + "&pid=" + page + "&api_key=" + this.apiKey + "&user_id=" + this.login);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string responseString = reader.ReadToEnd();

                document.LoadXml(responseString);
                root = document.DocumentElement;
                var posts = root.InnerXml;

                document.LoadXml(posts);
                post = document.DocumentElement;

                XmlNode node = document.DocumentElement;
                XmlNode sourceNode = node.SelectSingleNode("file_url");
                XmlNode tagsNode = node.SelectSingleNode("tags");
                XmlNode ratingNode = node.SelectSingleNode("rating");

                string url = sourceNode.InnerXml;
                rating = ratingNode.InnerXml;
                string postTags = tagsNode.InnerXml;

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
    }
}