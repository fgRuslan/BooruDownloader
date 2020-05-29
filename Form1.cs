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


namespace BooruDownloader
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();
        
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
            label4.Text = "";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 99999;
            toolTip1.InitialDelay = 250;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.detectButton, "Click here if you don't know what engine does the selected website uses. Keep in mind that engine detection feature is experimental.");
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

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            if (isDanbooruSite.Checked){//If it's a danbooru site
                DanEngine engine = new DanEngine();
                int postCount = engine.getPostCount(domainBox.Text, tagsBox.Text);
                Console.WriteLine(postCount);
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
                    for (int i = 1; i < postCount; i++)
                    {
                        await Task.Run(() => engine.downloadPosts(domainBox.Text, tagsBox.Text, i, checkBox1.Checked, ratingCheckBox.Checked));
                        label4.Text = Convert.ToString(postCount - i) + " left";
                    }
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Ready.";
                    label4.Text = "";
                    MessageBox.Show("Download compelete!", "BooruDownloader", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }
            }
            else
            {//If it's a gelbooru site

                GelEngine engine = new GelEngine();
                int postCount = engine.getPostCount(domainBox.Text, tagsBox.Text);
                Console.WriteLine(postCount);
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
                        await Task.Run(() => engine.downloadPosts(domainBox.Text, tagsBox.Text, i, checkBox1.Checked, ratingCheckBox.Checked));
                        label4.Text = Convert.ToString(postCount - i) + " left";
                    }
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Ready.";
                    label4.Text = "";
                    MessageBox.Show("Download compelete!", "BooruDownloader", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }

            }
        }

        private void detectButton_Click(object sender, EventArgs e)
        {
            string engine = Detector.detectEngine(domainBox.Text);
            switch (engine)
            {
                case "Dan":
                    isDanbooruSite.Checked = true;
                break;
                case "Gel":
                    isDanbooruSite.Checked = false;
                break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
