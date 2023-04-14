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

        public override async void DownloadImage(string url, string tags, bool keepOriginalNames, string rating)
        {
            string filename = rating + tags;

            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            filename = r.Replace(filename, "");

            string fullPath = Path.GetFullPath("./out/") + filename + "." + ExtensionFromUrl(url);

            try
            {
                int pathlen = Path.GetFullPath(fullPath).Length;
            }
            catch (PathTooLongException)//If path is invalid because the filename is too long
            {
                int difference = fullPath.Length - 259;//259 is the max path length
                filename = filename.Remove(filename.Length - difference);
                fullPath = Path.GetFullPath("./out/") + filename + "." + ExtensionFromUrl(url);
            }

            using (WebClient wc = new WebClient())
            {
                // wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                //                      "Windows NT 5.2; .NET CLR 1.0.3705;)");
                wc.Headers.Add("user-agent", ".NET Framework Test Client");
                try
                {
                    if (keepOriginalNames)
                        wc.DownloadFileTaskAsync(new System.Uri(url), fullPath).Wait();
                    else
                        wc.DownloadFileTaskAsync(new System.Uri(url), fullPath).Wait();
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
                    else if (rating == "e" || rating == "explicit")
                        rating = "nsfw ";
                    else if (rating == "s")
                        rating = "safe ";
                    else
                        rating = rating + " ";
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