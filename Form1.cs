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

        int alreadyDownloaded = 0;
        
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
#if DEBUG
            AllocConsole();
#endif
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
            EngineBase engine = Detector.detectEngine(domainBox.Text);
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

                int limitBoxText;
                try
                {
                    limitBoxText = int.Parse(limitBox.Text);
                }
                catch (Exception e1)
                {
                    limitBox.Text = "999";
                    limitBoxText = 999;
                }
                for (int i = 1; i < postCount; i++)
                {
                    if (alreadyDownloaded <= limitBoxText)
                    {
                        await Task.Run(() => engine.downloadPosts(domainBox.Text, tagsBox.Text, i, checkBox1.Checked, ratingCheckBox.Checked));
                        label4.Text = Convert.ToString(postCount - i) + " left";
                        alreadyDownloaded++;
                    }
                }
                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = "Ready.";
                label4.Text = "";
                MessageBox.Show("Download compelete!", "BooruDownloader", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
