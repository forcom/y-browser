using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using Core;
using System.Drawing;

namespace YWebView
{
    public class NetProcess
    {
        public Document CurrentPage { get; set; }
        public Uri Url { get; set; }

        Thread curNavigate = null;
        static Document workingPage;
        static Uri workingUrl;
        static bool workingdownload;

        static Queue<HttpWebResponse> DownloadQueue = new Queue<HttpWebResponse>();

        static int retry = 0;
        static void DoNavigate()
        {
            if (++retry > 5) return;

            HttpWebRequest wq = (HttpWebRequest)HttpWebRequest.Create(workingUrl);
            HttpWebResponse wr = null;

            try
            {
                wr = (HttpWebResponse)wq.GetResponse();
            }
            catch
            {
                // Throw Unable to Connect
                return;
            }

            //If it is not a html page, try to download as file.
            if (!wr.ContentType.Contains("text/html"))
            {
                DownloadQueue.Enqueue(wr);
                return;
            }

            try
            {
                Stream wrs = null;
                try
                {
                    wrs = wr.GetResponseStream();
                }
                catch (ObjectDisposedException)
                {
                    // If the connection was disconnected, Do Again.
                    DoNavigate();
                    return;
                }

                StreamReader sr = new StreamReader(wrs);
                string html = sr.ReadToEnd();

                workingPage = HtmlReader.GetDocument(html);
                workingdownload = true;
            }
            catch
            {
                // Error occured.
            }
        }

        public void Navigate(string _Url)
        {
            Uri _uri = null;
            try
            {
                _uri = new Uri(_Url);
            }
            catch (UriFormatException)
            {
                // Throw Incorrect Url
                return;
            }

            if (curNavigate != null && curNavigate.ThreadState == ThreadState.Running)
            {
                curNavigate.Abort();
                while (curNavigate.ThreadState == ThreadState.Running) ;
            }

            workingUrl = _uri;
            workingPage = null;
            workingdownload = false;
            retry = 0;
            curNavigate = new Thread(new ThreadStart(DoNavigate));
            curNavigate.Start();
        }

        public Image DownloadImage(string url)
        {
            Uri _uri = null;
            try
            {
                _uri = new Uri(workingUrl, url);
            }
            catch
            {
                // Throw Incorrect Url
                return null;
            }

            WebClient wc = new WebClient();
            Image img = null;

            try
            {
                byte[] img_data = wc.DownloadData(_uri);
                MemoryStream ms = new MemoryStream(img_data);
                img = new Bitmap(ms);
            }
            catch
            {
                // Throw Download Image Error
                return null;
            }
            return img;
        }

        public string GetUrl(string url)
        {
            Uri _url = null;
            try
            {
                _url = new Uri(workingUrl, url);
            }
            catch
            {
                return null;
            }

            return _url.OriginalString;
        }

        public void tmrNavigate_Tick(object sender, EventArgs e)
        {
            if (workingdownload == false)
                return;
            (sender as System.Windows.Forms.Timer).Enabled = false;

            CurrentPage = workingPage;
            Url = workingUrl;
        }
    }
}
