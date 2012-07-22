using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Core;

namespace YWebView
{
    public class DrawPage
    {
        public Bitmap Page { get; private set; }

        public Size DefaultSize { get; set; }

        public float LeftMargin { get; set; }
        public float TopMargin { get; set; }
        public float RightMargin { get; set; }

        public float XPosition { get; set; }
        public float YPosition { get; set; }
        
        public float IndentSize { get; set; }
        public int IndentLevel { get; set; }

        List<RectangleF> DrawArea = new List<RectangleF>();

        Stack<HtmlTag.ListType> CurrentListType = new Stack<HtmlTag.ListType>();
        Stack<int> OrderedListNumber = new Stack<int>();
        Stack<Font> DrawFont = new Stack<Font>();
        Stack<Brush> DrawBrush = new Stack<Brush>();
        Stack<int> MarkupRegion = new Stack<int>();

        public DrawPage()
        {
            DefaultSize = new Size(780, 600);
            LeftMargin = 20;
            TopMargin = 20;
            RightMargin = 20;
            YPosition = TopMargin;
            XPosition = LeftMargin;
            IndentSize = 40;
            IndentLevel = 0;

            Page = new Bitmap(DefaultSize.Width, DefaultSize.Height);

            DrawFont.Push(new Font("ＭＳ Ｐゴシック", 11));
            DrawBrush.Push(Brushes.Black);
            MarkupRegion.Push(0);
        }

        public void IncreaseHeight(int deltaHeight = 300)
        {
            Bitmap bmp = new Bitmap(Page.Width, Page.Height + deltaHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(Page, new Point(0, 0));
            g.Flush();
            Page.Dispose();
            Page = bmp;
        }

        public void IncreaseWidth(int deltaWidth)
        {
            Bitmap bmp = new Bitmap(Page.Width + deltaWidth, Page.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(Page, new Point(0, 0));
            g.Flush();
            Page.Dispose();
            Page = bmp;
        }

        public void DrawText(string text)
        {
            Graphics g = Graphics.FromImage(Page);

            Font fnt = DrawFont.Peek();
            Brush brush = DrawBrush.Peek();
            int hp, mp, tp;
            SizeF head, mid, tail;

            while (text != "")
            {
                hp = 0;
                tp = text.Length - 1;
                while (true)
                {
                    mp = (hp + tp) / 2;
                    head = g.MeasureString(text.Substring(0, hp + 1), fnt);
                    mid = g.MeasureString(text.Substring(0, mp + 1), fnt);
                    tail = g.MeasureString(text.Substring(0, tp + 1), fnt);

                    if (hp == 0 && XPosition + head.Width >= Page.Width - RightMargin)
                    {
                        YPosition += head.Height;

                        if (YPosition >= Page.Height)
                            IncreaseHeight();

                        XPosition = LeftMargin + IndentLevel * IndentSize;
                        continue;
                    }

                    if (XPosition + mid.Width >= Page.Width - RightMargin)
                    {
                        tp = mp - 1;
                        continue;
                    }

                    if (XPosition + tail.Width >= Page.Width - RightMargin)
                    {
                        hp = mp + 1;
                        continue;
                    }

                    if (YPosition + tail.Height >= Page.Height)
                        IncreaseHeight();

                    g.DrawString(text.Substring(0, tp + 1), fnt, brush, XPosition, YPosition);
                    DrawArea.Add(new RectangleF(XPosition, YPosition, tail.Width, tail.Height));

                    if (tp + 1 < text.Length)
                    {
                        text = text.Substring(tp + 1, text.Length - tp - 1);
                        XPosition = LeftMargin + IndentLevel * IndentSize;
                        YPosition += tail.Height;

                        if (YPosition >= Page.Height)
                            IncreaseHeight();
                    }
                    else
                    {
                        text = "";
                        XPosition += tail.Width;
                    }

                    break;
                }
            }
        }

        public Region DrawImage(Image image)
        {
            return null;
        }

        public void DrawNewLine()
        {
        }

        public void DrawHorizontalLine()
        {
        }

        public void DrawNewParagraph()
        {
        }

        public Region DrawFormField()
        {
            return null;
        }

        public void DrawHeader(int level, bool start = true)
        {
        }

        public void DrawList()
        {
        }

        public void BeginUnorderedList()
        {
        }

        public void EndUnorderedList()
        {
        }

        public void BeginOrderedList()
        {
        }

        public void EndOrderedList()
        {
        }

        public void BeginDirectory()
        {
        }

        public void EndDirectory()
        {
        }

        public void BeginMenu()
        {
        }

        public void EndMenu()
        {
        }

        public void BeginDefineTerm()
        {
        }

        public void EndDefineTerm()
        {
        }

        public void BeginMarkup()
        {
        }

        public Region EndMarkup()
        {
            return null;
        }
    }
}
