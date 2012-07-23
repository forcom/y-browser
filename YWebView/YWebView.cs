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
        public class HyperlinkInformation
        {
            public Region Location { get; set; }
            public string Name { get; set; }
            public string Href { get; set; }
            public string Title { get; set; }

            public HyperlinkInformation()
            {
                Location = new Region();
                Location.MakeEmpty();
                Name = null;
                Href = null;
                Title = null;
            }

            public HyperlinkInformation(Element elem)
            {
                Href = elem["HREF"];
                Name = elem["NAME"];
                Title = elem["TITLE"];
            }
        }

        public string Title { get; set; }
        public string StatusText { get; set; }
        public List<HyperlinkInformation> Hyperlinks { get; set; }

        NetProcess np = new NetProcess();
        Document curDoc = null;

        System.Text.RegularExpressions.Regex Whitespace = new System.Text.RegularExpressions.Regex(@"\s+");

        public YWebView()
        {
            InitializeComponent();
            Title = "";
            tmrNavigate.Tick += new EventHandler(np.tmrNavigate_Tick);
            Hyperlinks = new List<HyperlinkInformation>();
        }

        public void Navigate(string Url)
        {
            np.Navigate(Url);
            tmrNavigate.Enabled = true;
            tmrShowPage.Enabled = true;
        }

        void ShowPage()
        {
            List<HyperlinkInformation> _hyperlink = new List<HyperlinkInformation>();

            bool setTitle = false;
            bool isPRE = false;

            HyperlinkInformation curlink = null;

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
                                if (i.IsStartTag)
                                    Showing.BeginHeader(1);
                                else
                                    Showing.EndHeader();
                                break;
                            case "H2":
                                if (i.IsStartTag)
                                    Showing.BeginHeader(2);
                                else
                                    Showing.EndHeader();
                                break;
                            case "H3":
                                if (i.IsStartTag)
                                    Showing.BeginHeader(3);
                                else
                                    Showing.EndHeader();
                                break;
                            case "H4":
                                if (i.IsStartTag)
                                    Showing.BeginHeader(4);
                                else
                                    Showing.EndHeader();
                                break;
                            case "H5":
                                if (i.IsStartTag)
                                    Showing.BeginHeader(5);
                                else
                                    Showing.EndHeader();
                                break;
                            case "H6":
                                if (i.IsStartTag)
                                    Showing.BeginHeader(6);
                                else
                                    Showing.EndHeader();
                                break;
                            case "P":
                                Showing.DrawNewParagraph();
                                break;
                            case "PRE":
                                isPRE = i.IsStartTag;
                                if (isPRE)
                                {
                                    Showing.DrawNewParagraph();
                                }
                                break;
                            case "ADDRESS":
                                if (i.IsStartTag)
                                {
                                    Showing.DrawNewParagraph();
                                    ++Showing.IndentLevel;
                                    Showing.BeginMarkup(FontStyle.Italic);
                                }
                                else
                                {
                                    --Showing.IndentLevel;
                                    Showing.EndMarkup();
                                    Showing.DrawNewParagraph();
                                }
                                break;
                            case "BLOCKQUOTE":
                                if (i.IsStartTag)
                                {
                                    Showing.DrawNewParagraph();
                                    ++Showing.IndentLevel;
                                    Showing.BeginMarkup(FontStyle.Italic);
                                    Showing.DrawText(">>\"");
                                }
                                else
                                {
                                    Showing.DrawText("\"<<");
                                    --Showing.IndentLevel;
                                    Showing.EndMarkup();
                                    Showing.DrawNewParagraph();
                                }
                                break;
                            case "UL":
                                if (i.IsStartTag)
                                    Showing.BeginUnorderedList();
                                else
                                    Showing.EndUnorderedList();
                                break;
                            case "LI":
                                Showing.DrawList();
                                break;
                            case "OL":
                                if (i.IsStartTag)
                                    Showing.BeginOrderedList();
                                else
                                    Showing.EndOrderedList();
                                break;
                            case "DIR":
                                if (i.IsStartTag)
                                    Showing.BeginDirectory();
                                else
                                    Showing.EndDirectory();
                                break;
                            case "MENU":
                                if (i.IsStartTag)
                                    Showing.BeginMenu();
                                else
                                    Showing.EndMenu();
                                break;
                            case "DL":
                                if (i.IsStartTag)
                                    Showing.BeginDefinitionList();
                                else
                                    Showing.EndDefinitionList();
                                break;
                            case "DT":
                                Showing.DrawDefinitionTerm();
                                break;
                            case "DD":
                                Showing.DrawDefinitionDescription();
                                break;
                            case "FORM":
                                break;
                        }
                        break;
                    case Element.ElementType.Markup:
                        switch (i.Name.ToUpper())
                        {
                            case "CITE":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Italic);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "CODE":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Regular, true);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "EM":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Bold | FontStyle.Italic);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "KBD":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Regular, true);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "SAMP":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Regular, true);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "STRONG":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Bold);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "VAR":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Italic);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "B":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Bold);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "I":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Italic);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "TT":
                                if (i.IsStartTag)
                                    Showing.BeginMarkup(FontStyle.Regular, true);
                                else
                                    Showing.EndMarkup();
                                break;
                            case "A":
                                if (i.IsStartTag)
                                {
                                    curlink = new HyperlinkInformation(i);
                                    Showing.BeginHyperlink();
                                }
                                else
                                {
                                    curlink.Location = Showing.EndHyperlink();
                                    _hyperlink.Add(curlink);
                                }
                                break;
                        }
                        break;
                    case Element.ElementType.Object:
                        switch (i.Name.ToUpper())
                        {
                            case "IMG":
                                if (i["SRC"] == null)
                                    break;
                                Region rg = Showing.DrawImage(np.DownloadImage(i["SRC"]));
                                if (i["ALT"] != null)
                                {
                                    HyperlinkInformation hi = new HyperlinkInformation();
                                    hi.Location = rg;
                                    hi.Title = i["ALT"];
                                    _hyperlink.Add(hi);
                                }
                                break;
                            case "BR":
                                Showing.DrawNewLine();
                                break;
                            case "HR":
                                Showing.DrawHorizontalLine();
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
                            Showing.DrawText(text);
                        }
                        else
                        {
                            Showing.DrawRawText(text);
                        }
                        break;
                    default:
                        break;
                }
            }

            Page.Image = Showing.Page;
            Hyperlinks = _hyperlink;
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

        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            bool flag = false;
            foreach (var i in Hyperlinks)
            {
                if (i.Location != null && i.Location.IsVisible(e.Location))
                {
                    flag = true;
                    if (i.Title != null)
                    {
                        toolTip.SetToolTip(Page, i.Title);
                    }
                    if (i.Href != null)
                    {
                        StatusText = np.GetUrl(i.Href);
                        Page.Cursor = Cursors.Hand;
                    }
                }
            }
            if (!flag)
            {
                StatusText = "";
                Page.Cursor = Cursors.Default;
            }
        }

        private void Page_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var i in Hyperlinks)
            {
                if (i.Location != null && i.Location.IsVisible(e.Location))
                {
                    if (i.Href != null)
                    {
                        Navigate(np.GetUrl(i.Href));
                    }
                }
            }
        }
    }
}
