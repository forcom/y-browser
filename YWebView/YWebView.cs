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

        System.Text.RegularExpressions.Regex Whitespace = new System.Text.RegularExpressions.Regex(@"\s+");

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
            bool setTitle = false;
            bool isPRE = false;

            DrawPage Showing = new DrawPage();

            curDoc = np.CurrentPage;
            StringBuilder sb = new StringBuilder();
            foreach (var i in curDoc.Items)
            {
                switch (i.Type)
                {
                    case Element.ElementType.Unknown:
                        break;
                    case Element.ElementType.Special:
                        break;
                    case Element.ElementType.Structure:
                        switch (i.Name.ToUpper())
                        {
                            case "TITLE":
                                setTitle = i.IsStartTag;
                                break;
                            case "H1":
                                break;
                            case "H2":
                                break;
                            case "H3":
                                break;
                            case "H4":
                                break;
                            case "H5":
                                break;
                            case "H6":
                                break;
                            case "P":
                                break;
                            case "PRE":
                                isPRE = i.IsStartTag;
                                break;
                            case "ADDRESS":
                                break;
                            case "BLOCKQUOTE":
                                break;
                            case "UL":
                                break;
                            case "LI":
                                break;
                            case "OL":
                                break;
                            case "DIR":
                                break;
                            case "MENU":
                                break;
                            case "DL":
                                break;
                            case "DT":
                                break;
                            case "DD":
                                break;
                            case "FORM":
                                break;
                        }
                        break;
                    case Element.ElementType.Markup:
                        switch (i.Name.ToUpper())
                        {
                            case "CITE":
                                break;
                            case "CODE":
                                break;
                            case "EM":
                                break;
                            case "KBD":
                                break;
                            case "SAMP":
                                break;
                            case "STRONG":
                                break;
                            case "VAR":
                                break;
                            case "B":
                                break;
                            case "I":
                                break;
                            case "TT":
                                break;
                            case "A":
                                break;
                        }
                        break;
                    case Element.ElementType.Object:
                        switch (i.Name.ToUpper())
                        {
                            case "IMG":
                                break;
                            case "BR":
                                break;
                            case "HR":
                                break;
                            case "INPUT":
                                break;
                            case "SELECT":
                                break;
                            case "OPTION":
                                break;
                            case "TEXTAREA":
                                break;
                        }
                        break;
                    case Element.ElementType.Text:
                        if (setTitle)
                        {
                            Title = i.Name;
                            break;
                        }
                        string text = i.Name;
                        if (!isPRE)
                        {
                            text = Whitespace.Replace(text, " ");
                        }
                        Showing.DrawText(text);
                        break;
                    default:
                        break;
                }
            }

            Page.Image = Showing.Page;
        }

        private void tmrShowPage_Tick(object sender, EventArgs e)
        {
            if (tmrNavigate.Enabled) return;
            tmrShowPage.Enabled = false;
            ShowPage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Navigate("http://www.w3.org/MarkUp/html-spec/");
        }

        private void YWebView_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
