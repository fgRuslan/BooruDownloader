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

namespace BooruDownloader
{
    class GelEngine
    {

        XmlDocument doc = new XmlDocument();
        XmlElement root;
        string url;

        string ExtFromURL(string line)
        {
            var ext = "";
            var match = Regex.Match(line, "(?:)\\.[\\d\\w]+$");
            if (match.Success)
                ext = match.Value;
            return ext;
        }
        string FnameFromURL(string line)
        {
            var fname = "";
            var match = Regex.Match(line, "(?:)[\\d\\w]+\\.[\\d\\w]+$");
            if (match.Success)
                fname = match.Value;
            return fname;
        }


        private void downloadImage(string url, string tags, bool keepOriginalNames)
        {
            using (WebClient wc = new WebClient())
            {
                if (keepOriginalNames)
                    wc.DownloadFileAsync(new System.Uri(url), "./out/" + FnameFromURL(url));
                else
                    wc.DownloadFileAsync(new System.Uri(url), "./out/" + tags + ExtFromURL(url));
            }
        }

        public int getPostCount(string domain, string tags)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tags);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //AllocConsole();
            string result = "";
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();
                var match = Regex.Match(responseString, "count=\"([\\s\\S]+?)\" ");
                if (match.Success)
                {
                    Console.WriteLine(match.Groups[1].Value + " posts found");
                    result = match.Groups[1].Value;
                }
            }
            return Convert.ToInt32(result);
        }
        public string downloadPosts(string domain, string tags, int page, bool keepOriginalNames)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tags + "&pid=" + page);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();

                doc.LoadXml(responseString);
                root = doc.DocumentElement;
                var posts = root.InnerXml;

                doc.LoadXml(posts);
                root = doc.DocumentElement;

                url = root.Attributes["file_url"].Value;
                tags = root.Attributes["tags"].Value;
                downloadImage(url, tags, keepOriginalNames);
            }
            return url;
        }
    }
}
