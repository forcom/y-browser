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

        public int LeftMargin { get; set; }
        public int TopMargin { get; set; }

        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public int IndentSize { get; set; }
        public int IndentLevel { get; set; }

        List<Rectangle> DrawArea = new List<Rectangle>();

        Stack<HtmlTag.ListType> CurrentListType = new Stack<HtmlTag.ListType>();
        Stack<int> OrderedListNumber = new Stack<int>();
        Stack<Font> DrawFont = new Stack<Font>();
        Stack<int> MarkupRegion = new Stack<int>();

        public DrawPage()
        {
            DefaultSize = new Size(780, 600);
            LeftMargin = 20;
            TopMargin = 20;
            XPosition = TopMargin;
            YPosition = LeftMargin;
            IndentSize = 40;
            IndentLevel = 0;

            DrawFont.Push(new Font("ＭＳ Ｐゴシック", 11));
            MarkupRegion.Push(0);
        }

        public void IncreaseHeight(int deltaHeight = 300)
        {
        }

        public void IncreaseWidth(int deltaWidth)
        {
        }

        public Region DrawText(string text)
        {
            return null;
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
