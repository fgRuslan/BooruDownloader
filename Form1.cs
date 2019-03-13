using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;


namespace GelDownloader
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        XmlDocument doc = new XmlDocument();
        XmlElement root;
        string url;
        string tags;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool exists = System.IO.Directory.Exists("./out/");
            if(!exists)
                System.IO.Directory.CreateDirectory("./out/");

            tagsBox.GotFocus += new EventHandler(this.TagsGotFocus);
            tagsBox.LostFocus += new EventHandler(this.TagsLostFocus);
        }

        public void TagsGotFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "tags separated by whitespace")
            {
                tb.Text = "";
                tb.ForeColor = Color.Black;
            }
        }

        public void TagsLostFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "tags separated by whitespace";
                tb.ForeColor = Color.DarkGray;
            }
        }

        private int getPostCount()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domainBox.Text + "/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tagsBox.Text);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //AllocConsole();
            string result = "";
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();
                var match = Regex.Match(responseString, "count=\"([\\s\\S]+?)\" ");
                //var match = Regex.Match(responseString, "file_url=[^ ]+");
                if (match.Success)
                {
                    Console.WriteLine(match.Groups[1].Value + " posts found");
                    result = match.Groups[1].Value;
                }
            }
            return Convert.ToInt32(result);
        }

        private string downloadPosts(int page)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domainBox.Text + "/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tagsBox.Text + "&pid=" + page);
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
                downloadImage(url, tags);
            }
            return url;
        }
        string ExtFromURL(string line)
        {
            var ext = "";
            var match = Regex.Match(line, "(?:)\\.[\\d\\w]+$");
            if(match.Success)
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


        private void downloadImage(string url, string tags)
        {
                    using (WebClient wc = new WebClient())
                    {
                        if(checkBox1.Checked)
                            wc.DownloadFileAsync(new System.Uri(url), "./out/" + FnameFromURL(url));//TODO: Files must be downloaded to out/ directory.
                        else
                            wc.DownloadFileAsync(new System.Uri(url), "./out/" + tags + ExtFromURL(url));//TODO: Files must be downloaded to out/ directory.
                    }
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {

            int postCount = getPostCount();
            if (postCount == 0)
            {
                Console.WriteLine("No posts found by tag " + tagsBox.Text);
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "No posts found.";
            }
            else
            {
                statusLabel.ForeColor = Color.Blue;
                statusLabel.Text = "Downloading...";
                for (int i = 0; i < postCount; i++)
                {
                    downloadPosts(i);
                }
                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = "Ready.";
                MessageBox.Show("Download compelete!", "GelDownloader", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);    
            }
        }
    }
}
