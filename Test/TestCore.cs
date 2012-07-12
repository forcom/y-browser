using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;

namespace Test
{
    /// <summary>
    /// TestCore の概要の説明
    /// </summary>
    [TestClass]
    public class TestCore
    {
        public TestCore()
        {
            //
            // TODO: コンストラクター ロジックをここに追加します
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        //
        // テストを作成する際には、次の追加属性を使用できます:
        //
        // クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        string MakeHtml(Core.Document doc)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var i in doc.Items)
            {
                switch (i.Type)
                {
                    case Element.ElementType.Special:
                    case Element.ElementType.Object:
                        sb.Append('<');
                        if (!i.IsStartTag) sb.Append('/');
                        sb.Append(i.Name);
                        foreach (var j in i.Attributes.Items.Values)
                        {
                            sb.Append(' ');
                            sb.Append(j.Name);
                            if (j.Value != "")
                            {
                                sb.Append("=\"");
                                sb.Append(j.Value);
                                sb.Append("\"");
                            }
                        }
                        sb.Append(">\n");
                        break;
                    case Element.ElementType.Text:
                        sb.Append(i.Name);
                        break;
                    case Element.ElementType.Structure:
                        sb.Append("\n<");
                        if (!i.IsStartTag) sb.Append('/');
                        sb.Append(i.Name);
                        foreach (var j in i.Attributes.Items.Values)
                        {
                            sb.Append(' ');
                            sb.Append(j.Name);
                            if (j.Value != "")
                            {
                                sb.Append("=\"");
                                sb.Append(j.Value);
                                sb.Append("\"");
                            }
                        }
                        sb.Append(">\n");
                        break;
                    case Element.ElementType.Markup:
                        sb.Append("<");
                        if (!i.IsStartTag) sb.Append('/');
                        sb.Append(i.Name);
                        foreach (var j in i.Attributes.Items.Values)
                        {
                            sb.Append(' ');
                            sb.Append(j.Name);
                            if (j.Value != "")
                            {
                                sb.Append("=\"");
                                sb.Append(j.Value);
                                sb.Append("\"");
                            }
                        }
                        sb.Append(">");
                        break;
                }
            }
            return sb.ToString();
        }

        [TestMethod]
        public void TestHtmlReader()
        {
            string html = @"<!DOCTYPE HTML PUBLIC ""-//IETF//DTD HTML 2.0//EN"">
<HTML>
<!-- Here’s a good place to put a comment. -->
<HEAD>
<TITLE>Structural Example</TITLE>
</HEAD><BODY>
<H1>First Header</H1>
<P>This is a paragraph in the example HTML file. Keep in mind
that the title does not appear in the document text, but that
the header (defined by H1) does.</P>
<OL>
<LI>First item in an ordered list.
<LI>Second item in an ordered list.
<UL COMPACT>
<LI> Note that lists can be nested;
<LI> Whitespace may be used to assist in reading the
HTML source.
</UL>
<LI>Third item in an ordered list.
</OL>
<P>This is an additional paragraph. Technically, end tags are
not required for paragraphs, although they are allowed. You can
include character highlighting in a paragraph. <EM>This sentence
of the paragraph is emphasized.</EM> Note that the &lt;/P&gt;
end tag has been omitted.
<P>
<IMG SRC =""triangle.xbm"" alt=""Warning: "">
Be sure to read these <b>bold instructions</b>.
</BODY></HTML>";
            HtmlReader.GetDocument(html);
        }

        [TestMethod]
        public void TestCoreDataStructure()
        {
            /* Test with 3.4 Example HTML Document in RFC 1866(http://www.ietf.org/rfc/rfc1866.txt)
             *
             * <!DOCTYPE HTML PUBLIC "-//IETF//DTD HTML 2.0//EN">
             * <HTML>
             * <!-- Here’s a good place to put a comment. -->
             * <HEAD>
             * <TITLE>Structural Example</TITLE>
             * </HEAD><BODY>
             * <H1>First Header</H1>
             * <P>This is a paragraph in the example HTML file. Keep in mind
             * that the title does not appear in the document text, but that
             * the header (defined by H1) does.</P>
             * <OL>
             * <LI>First item in an ordered list.
             * <LI>Second item in an ordered list.
             * <UL COMPACT>
             * <LI> Note that lists can be nested;
             * <LI> Whitespace may be used to assist in reading the
             * HTML source.
             * </UL>
             * <LI>Third item in an ordered list.
             * </OL>
             * <P>This is an additional paragraph. Technically, end tags are
             * not required for paragraphs, although they are allowed. You can
             * include character highlighting in a paragraph. <EM>This sentence
             * of the paragraph is emphasized.</EM> Note that the &lt;/P&gt;
             * end tag has been omitted.
             * <P>
             * <IMG SRC ="triangle.xbm" alt="Warning: ">
             * Be sure to read these <b>bold instructions</b>.
             * </BODY></HTML>
             */
            Element doctype = new Element("DOCTYPE", Element.ElementType.Special);
            doctype.Attributes["HTML"] = "";
            doctype.Attributes["PUBLIC"] = "";
            doctype.Attributes["\"-//IETF//DTD HTML 2.0//EN\""] = "";
            Element html_s = new Element("HTML");
            Element comment = new Element("--", Element.ElementType.Special);
            comment.Attributes["Here’s a good place to put a comment."] = "";
            comment.Attributes["--"] = "";
            Element head_s = new Element("HEAD");
            Element title_s = new Element("TITLE");
            Element txt1 = new Element("Structural Example", Element.ElementType.Text);
            Element title_e = new Element("TITLE", Element.ElementType.Structure, false);
            Element head_e = new Element("HEAD", Element.ElementType.Structure, false);
            Element body_s = new Element("BODY");
            Element h1_s = new Element("H1");
            Element txt2 = new Element("First Header", Element.ElementType.Text);
            Element h1_e = new Element("H1", Element.ElementType.Structure, false);
            Element p1_s = new Element("P");
            Element txt3 = new Element("This is a paragraph in the example HTML file. Keep in mind that the title does not appear in the document text, but that the header (defined by H1) does.", Element.ElementType.Text);
            Element p1_e = new Element("P", Element.ElementType.Structure, false);
            Element ol_s = new Element("OL");
            Element li1 = new Element("LI");
            Element txt4 = new Element("First item in an ordered list.", Element.ElementType.Text);
            Element txt5 = new Element("Second item in an ordered list.", Element.ElementType.Text);
            Element ul_s = new Element("UL");
            ul_s.Attributes["COMPACT"] = "";
            Element li3 = new Element("LI");
            Element txt6 = new Element("Note that lists can be nested;", Element.ElementType.Text);
            Element li4 = new Element("LI");
            Element txt7 = new Element("Whitespace may be used to assist in reading the HTML source.", Element.ElementType.Text);
            Element ul_e = new Element("UL", Element.ElementType.Structure, false);
            Element li5 = new Element("LI");
            Element txt8 = new Element("Third item in an ordered list.", Element.ElementType.Text);
            Element ol_e = new Element("OL", Element.ElementType.Structure, false);
            Element p2 = new Element("P");
            Element txt9 = new Element("This is an additional paragraph. Technically, end tags are not required for paragraphs, although they are allowed. You can include character highlighting in a paragraph.", Element.ElementType.Text);
            Element em_s = new Element("EM", Element.ElementType.Markup);
            Element txt10 = new Element("This sentence of the paragraph is emphasized.", Element.ElementType.Text);
            Element em_e = new Element("EM", Element.ElementType.Markup, false);
            Element txt11 = new Element("Note that the &lt;/P&gt; end tag has been omitted.", Element.ElementType.Text);
            Element p3 = new Element("P");
            Element img = new Element("IMG", Element.ElementType.Object);
            img.Attributes["SRC"] = "triangle.xbm";
            img.Attributes["alt"] = "Warning: ";
            Element txt12 = new Element("Be sure to read these ", Element.ElementType.Text);
            Element b_s = new Element("b", Element.ElementType.Markup);
            Element txt13 = new Element("bold instructions", Element.ElementType.Text);
            Element b_e = new Element("b", Element.ElementType.Markup, false);
            Element txt14 = new Element(".", Element.ElementType.Text);
            Element body_e = new Element("BODY", Element.ElementType.Structure, false);
            Element html_e = new Element("HTML", Element.ElementType.Structure, false);

            Document doc = new Document();
            doc.Items.Add(doctype);
            doc.Items.Add(html_s);
            doc.Items.Add(comment);
            doc.Items.Add(head_s);
            doc.Items.Add(title_s);
            doc.Items.Add(txt1);
            doc.Items.Add(title_e);
            doc.Items.Add(head_e);
            doc.Items.Add(body_s);
            doc.Items.Add(h1_s);
            doc.Items.Add(txt2);
            doc.Items.Add(h1_e);
            doc.Items.Add(p1_s);
            doc.Items.Add(txt3);
            doc.Items.Add(p1_e);
            doc.Items.Add(ol_s);
            doc.Items.Add(li1);
            doc.Items.Add(txt4);
            doc.Items.Add(txt5);
            doc.Items.Add(ul_s);
            doc.Items.Add(li3);
            doc.Items.Add(txt6);
            doc.Items.Add(li4);
            doc.Items.Add(txt7);
            doc.Items.Add(ul_e);
            doc.Items.Add(li5);
            doc.Items.Add(txt8);
            doc.Items.Add(ol_e);
            doc.Items.Add(p2);
            doc.Items.Add(txt9);
            doc.Items.Add(em_s);
            doc.Items.Add(txt10);
            doc.Items.Add(em_e);
            doc.Items.Add(txt11);
            doc.Items.Add(p3);
            doc.Items.Add(img);
            doc.Items.Add(txt12);
            doc.Items.Add(b_s);
            doc.Items.Add(txt13);
            doc.Items.Add(b_e);
            doc.Items.Add(txt14);
            doc.Items.Add(body_e);
            doc.Items.Add(html_e);

            string Result = MakeHtml(doc);
        }
    }
}
