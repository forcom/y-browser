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
                        {
                            g.Flush();
                            IncreaseHeight();
                            g = Graphics.FromImage(Page);
                        }

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
                    {
                        g.Flush();
                        IncreaseHeight();
                        g = Graphics.FromImage(Page);
                    }

                    g.DrawString(text.Substring(0, tp + 1), fnt, brush, XPosition, YPosition);
                    DrawArea.Add(new RectangleF(XPosition, YPosition, tail.Width, tail.Height));

                    if (tp + 1 < text.Length)
                    {
                        text = text.Substring(tp + 1, text.Length - tp - 1);
                        XPosition = LeftMargin + IndentLevel * IndentSize;
                        YPosition += tail.Height;

                        if (YPosition >= Page.Height)
                        {
                            g.Flush();
                            IncreaseHeight();
                            g = Graphics.FromImage(Page);
                        }
                    }
                    else
                    {
                        text = "";
                        XPosition += tail.Width;
                    }

                    break;
                }
            }
            g.Flush();
        }

        public void DrawRawText(string text)
        {
            Graphics g = Graphics.FromImage(Page);

            Font fnt = DrawFont.Peek();
            Brush brush = DrawBrush.Peek();

            string[] lines = text.Split(new char[] { '\n', '\r' });

            SizeF size = new SizeF();

            foreach (var i in lines)
            {
                size = g.MeasureString(i, fnt);
                XPosition = LeftMargin + IndentLevel * IndentSize;

                if (LeftMargin + size.Width >= Page.Width - RightMargin)
                {
                    g.Flush();
                    IncreaseWidth((int)(LeftMargin + size.Width - Page.Width + RightMargin));
                    g = Graphics.FromImage(Page);
                }

                if (YPosition + size.Height >= Page.Height)
                {
                    g.Flush();
                    IncreaseHeight();
                    g = Graphics.FromImage(Page);
                }

                g.DrawString(i, fnt, brush, XPosition, YPosition);

                XPosition += size.Width;
                YPosition += size.Height;
            }

            if (!size.IsEmpty)
                YPosition -= size.Height;
            g.Flush();
        }

        public Region DrawImage(Image image)
        {
            Region rg = new Region();
            RectangleF rc = new RectangleF();
            rg.MakeEmpty();
            Graphics g = Graphics.FromImage(Page);
            
            if (XPosition + image.Width >= Page.Width)
            {
                IncreaseWidth((int)(XPosition + image.Width - Page.Width + RightMargin));
                g = Graphics.FromImage(Page);
            }

            if (YPosition + image.Height >= Page.Height)
            {
                IncreaseHeight((int)(YPosition + image.Height - Page.Height + 300));
                g = Graphics.FromImage(Page);
            }

            g.DrawImage(image, XPosition, YPosition);

            rc = new RectangleF(XPosition, YPosition, image.Width, image.Height);
            DrawArea.Add(rc);
            rg.Union(rc);

            XPosition = LeftMargin + IndentLevel * IndentSize;
            YPosition += image.Height;

            g.Flush();

            return rg;
        }

        public void DrawNewLine()
        {
            Graphics g = Graphics.FromImage(Page);
            var size = g.MeasureString(" ", DrawFont.Peek());
            YPosition += size.Height;
            XPosition = LeftMargin + IndentLevel * IndentSize;
        }

        public void DrawHorizontalLine()
        {
            Graphics g = Graphics.FromImage(Page);
            var size = g.MeasureString(" ", DrawFont.Peek());
            if (YPosition + size.Height + 10 >= Page.Height)
            {
                IncreaseHeight();
                g = Graphics.FromImage(Page);
            }
            YPosition += size.Height;
            XPosition = LeftMargin + IndentLevel * IndentSize;
            g.DrawLine(new Pen(Color.Black, 3.0f), XPosition, YPosition, Page.Width - RightMargin, YPosition);
            g.Flush();
            YPosition += 6;
        }

        public void DrawNewParagraph()
        {
            Graphics g = Graphics.FromImage(Page);
            var size = g.MeasureString(" ", DrawFont.Peek());
            YPosition += size.Height;
            XPosition = LeftMargin + IndentLevel * IndentSize;
        }

        public Region DrawFormField()
        {
            return null;
        }

        public void DrawList()
        {
            DrawNewLine();
            switch (CurrentListType.Peek())
            {
                case HtmlTag.ListType.OL:
                    int lev = OrderedListNumber.Pop();
                    DrawText(lev.ToString() + ".　");
                    OrderedListNumber.Push(lev + 1);
                    break;
                case HtmlTag.ListType.UL:
                    DrawText("●　");
                    break;
                case HtmlTag.ListType.DIR:
                    DrawText("・　");
                    break;
                case HtmlTag.ListType.MENU:
                    DrawText("・　");
                    break;
                default:
                    break;
            }
        }

        public void BeginHeader(int level)
        {
            Font prev = DrawFont.Peek();
            Font fnt = new Font(prev.FontFamily, 23 - level * 2, prev.Style | FontStyle.Bold);
            DrawFont.Push(fnt);
            DrawNewLine();
        }

        public void EndHeader()
        {
            DrawNewParagraph();
            DrawFont.Pop();
        }

        public void BeginUnorderedList()
        {
            CurrentListType.Push(HtmlTag.ListType.UL);
            ++IndentLevel;
            DrawNewParagraph();
        }

        public void EndUnorderedList()
        {
            CurrentListType.Pop();
            --IndentLevel;
            DrawNewParagraph();
        }

        public void BeginOrderedList()
        {
            CurrentListType.Push(HtmlTag.ListType.OL);
            OrderedListNumber.Push(1);
            ++IndentLevel;
            DrawNewParagraph();
        }

        public void EndOrderedList()
        {
            CurrentListType.Pop();
            OrderedListNumber.Pop();
            --IndentLevel;
            DrawNewParagraph();
        }

        public void BeginDirectory()
        {
            CurrentListType.Push(HtmlTag.ListType.DIR);
            ++IndentLevel;
            DrawNewParagraph();
        }

        public void EndDirectory()
        {
            CurrentListType.Pop();
            --IndentLevel;
            DrawNewParagraph();
        }

        public void BeginMenu()
        {
            CurrentListType.Push(HtmlTag.ListType.MENU);
            ++IndentLevel;
            DrawNewParagraph();
        }

        public void EndMenu()
        {
            CurrentListType.Pop();
            --IndentLevel;
            DrawNewParagraph();
        }

        public void BeginDefinitionList()
        {
            DrawNewParagraph();
            CurrentListType.Push(HtmlTag.ListType.DL);
            OrderedListNumber.Push(IndentLevel);
        }

        public void EndDefinitionList()
        {
            CurrentListType.Pop();
            IndentLevel = OrderedListNumber.Pop();
            DrawNewParagraph();
        }

        public void DrawDefinitionTerm()
        {
            IndentLevel = OrderedListNumber.Peek();
            DrawNewParagraph();
        }

        public void DrawDefinitionDescription()
        {
            IndentLevel = OrderedListNumber.Peek() + 1;
            DrawNewLine();
        }

        public void BeginMarkup(FontStyle style, bool monospace = false)
        {
            Font prev = DrawFont.Peek();
            Font fnt = null;
            if (!monospace)
            {
                fnt = new Font(prev, prev.Style | style);
            }
            else
            {
                fnt = new Font("ＭＳ ゴシック", prev.Size, prev.Style | style);
            }
            DrawFont.Push(fnt);
        }

        public void EndMarkup()
        {
            DrawFont.Pop();
        }

        public void BeginHyperlink()
        {
            Font prev = DrawFont.Peek();
            Font fnt = new Font(prev, prev.Style | FontStyle.Underline);
            DrawFont.Push(fnt);
            DrawBrush.Push(Brushes.Blue);
            MarkupRegion.Push(DrawArea.Count);
        }

        public Region EndHyperlink()
        {
            DrawFont.Pop();
            DrawBrush.Pop();

            int s = MarkupRegion.Pop();

            if (DrawArea.Count - s == 0)
                return null;

            Region rg = new Region();
            rg.MakeEmpty();
            for (int i = s; i < DrawArea.Count; ++i)
            {
                rg.Union(DrawArea[i]);
            }
            return rg;
        }
    }
}
