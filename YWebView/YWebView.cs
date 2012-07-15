using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using Core;

namespace YWebView
{
    public partial class YWebView : UserControl
    {
        public string Title { get; set; }

        NetProcess np = new NetProcess();
        Document curDoc = null;

        public YWebView()
        {
            InitializeComponent();
            Title = "";
            tmrNavigate.Tick += new EventHandler(np.tmrNavigate_Tick);
        }

        public void Navigate(string Url)
        {
            np.Navigate(Url);
            tmrNavigate.Enabled = true;
            tmrShowPage.Enabled = true;
        }

        void ShowPage()
        {
            curDoc = np.CurrentPage;
        }

        private void tmrShowPage_Tick(object sender, EventArgs e)
        {
            if (tmrNavigate.Enabled) return;
            /*Thread thr = new Thread(new ThreadStart(ShowPage));
            thr.Start();*/
            tmrShowPage.Enabled = false;
            ShowPage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Navigate("http://www.w3.org/MarkUp/html-spec/");
        }
    }
}
