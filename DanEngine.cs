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

        public string downloadPosts(string domain, string tags, int page, bool keepOriginalNames)
        {
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
                if (sourceNode == null)
                {
                    return "";
                }
                url = sourceNode.InnerXml;
                var tagstring = tagsNode.InnerXml;
                Console.WriteLine(url);
                downloadImage(url, tagstring, keepOriginalNames);
            }
            return url;
        }

        public int getPostCount(string domain, string tags)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/posts.xml?page=dapi&s=post&q=index&tags=" + tags);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            AllocConsole();
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
