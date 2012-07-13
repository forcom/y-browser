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

#if DEBUG
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
            string html2 = @"<!DOCTYPE HTML PUBLIC ""-//IETF//DTD HTML 2.0 Strict//EN"">
<HTML>
<HEAD>
  <TITLE>HTML Working Group of the IETF (at W3C)</TITLE>
</HEAD>
<BODY>
<P>
<A HREF=""../../""><IMG BORDER=""0"" ALT=""W3C"" SRC=""../../Icons/WWW/w3c_home""
    ALT=""W3C""></A> | <A HREF=""../"">HTML</A>
<H1>
  <A HREF=""http://www.ietf.org/""><IMG BORDER=""0"" SRC=""../../Icons/WWW/ietf_48x84.gif""
      ALT=""IETF"" WIDTH=""84"" HEIGHT=""48""></A> HTML WG
</H1>
<H2>
  NOTE: This working group is closed
</H2>
<PRE>
Date: Thu, 12 Sep 1996 08:52:30 -0500
Message-Id: &lt;2.2.32.19960912135230.00b6c3f8@spyglass.com&gt;
To: html-wg@w3.org
From: ""Eric W. Sink"" <eric@spyglass.com>
Subject: This WG is now closed
</PRE>
<P>
See: <A href=""../Activity"">W3C HTML Activity</A> for HTML development status.
<BLOCKQUOTE>
  The HTML Working Group is chartered firstly to describe, and secondly to
  develop, the HyperText Markup Language (HTML). The group's work is to be
  based on existing practice on the Internet, and will make due reference to
  the SGML standard.
  <ADDRESS>
    from the
    <A HREF=""http://www.ietf.cnri.reston.va.us/proceedings/94dec/charters/html-charter.html"">charter
    of the HTML WG</A><BR>
    as of IETF 31, Dec 1994
  </ADDRESS>
</BLOCKQUOTE>
<P>
If you are new to the <A href=""http://www.ietf.org/"">IETF</A>, you should
probably do some background reading. I recommend:
<UL>
  <LI>
    <A href=""ftp://ds.internic.net/ietf/1wg-guidelines.txt"">IETF Working Group
    Process</A>
  <LI>
    <A href=""ftp://ds.internic.net/ietf/1id-guidelines.txt"">Guidelines to Draft
    Authors</A>
  <LI>
    <A href=""http://www.ietf.cnri.reston.va.us/tao.html"">The TAO of the IETF</A>
</UL>
<P>
IETF working groups exist for the sole purpose of drafting, revising, &nbsp;and
reviewing Internet Standards. They have a focused charter and milestones.
In order to conduct the business of the working group effectively, discussion
of items not on the charter is prohibited (this rule is enforced by the working
group chair, and sometimes the members. <STRONG>You have been warned</STRONG>.)
There are a&nbsp;number of <A HREF=""../#discussion"">other HTML and WWW discussion
forums</A>, such as mailing lists and USENET newsgroups, where discussions
of&nbsp;philosophy, proposed features, and announcements of WWW systems and
resources are welcome.
<H2>
  Mailing List
</H2>
<P>
The business of the working group is conducted on a&nbsp;public mailing list,
<TT>&lt;html-wg@w3.org&gt;</TT> (formerly html-wg@oclc.org).
<H2>
  Background and Archives
</H2>
<DL>
  <DT>
    <A HREF=""http://www.ietf.cnri.reston.va.us/html.charters/html-charter.html"">HTML
    working group charter</A>
  <DD>
    maintained by the IETF secretariate
    <TT>&lt;ietf-web@cnri.reston.va.us&gt;</TT>
  <DT>
    <A HREF=""http://listserv.heanet.ie/html-wg.html""><!--
    <A HREF=""http://www.acl.lanl.gov/HTML_WG/archives.html"">-->Hypertext Archive
    of HTML-WG mailing list</A>
  <DD>
    formerly maintained by Ron Daniel<BR>
    <!--no longer there, but there are
    <A HREF=""http://www.altavista.com/cgi-bin/query?pg=q&amp;kl=XX&amp;q=link%3Ahttp%3A%2F%2Fwww.acl.lanl.gov%2FHTML_WG%2F"">lots
    of links to it</A>-->
  <DT>
    <A HREF=""http://www.ics.uci.edu/pub/ietf/html/"">HTML-WG: Abstracts and Related
    Info</A>
  <DD>
    maintained by Roy Fielding
  <DT>
    <A HREF=""http://www.acl.lanl.gov/HTML/html-archive.subject-index.html"">HTML
    Implementors Group mailing list archive</A>
  <DD>
    Archive of messages from June 14, 1994 to October 14, 1994, dealing with
    revisions to the HTML 2.0 specifications.
    (<A HREF=""http://www.devry-phx.edu/webresrc/webmstry/langtech.htm"">source</A>)
</DL>
<H2>
  Principals
</H2>
<DL>
  <DT>
    HTML Working Group Chair
  <DD>
    Eric Sink <TT>esink@spyglass.com</TT>
  <DT>
    Editor of the HTML 2.n Specification:
  <DD>
    Dan Connolly <TT>connolly@w3.org</TT>
  <DT>
    Editor of the 3.n Specification(s)
  <DD>
    Dave Raggett <TT>dsr@w3.org</TT>
</DL>
<H2>
  Proposing New Features
</H2>
<P>
Those who would propose new features or modify old ones are urged to discuss
their ideas with experienced HTML implementors before going public. In any
case, the following issues should be addressed in any proposal:
<UL>
  <LI>
    a statement of the problem as you see it
  <LI>
    a proposed solution
  <LI>
    a demonstration that this solution is globally cost-effective without being
    locally prohibitive (i.e. the sum of all the effort of deploying this solution
    is less than the cost of dealing with the problem with existing technology,
    and yet no one party bears too much of the burden. For example, if you require
    every information provider to do something, it had better be minimal.)
  <LI>
    a discussion of graceful deployment and interoperability issues.
</UL>
<H3>
  <STRONG>**** NOTE WELL **** </STRONG>
</H3>
<P>
Your ideas will achieve greater credence if you take the time and effort
to disseminate proposals as an internet draft. Don't forget to follow the
internet draft guidelines:
<A HREF=""ftp://ietf.cnri.reston.va.us/internet-drafts/1id-guidelines.txt""><CITE>Guidelines
to Authors of Internet-Drafts</CITE></A>
<BLOCKQUOTE>
  ""We value your opinion, really we do, but we're going to put a stiff tax
  on its expression, so that we won't have to hear it very often.""
  <ADDRESS>
    -name withheld to protect the guilty ;-)
  </ADDRESS>
</BLOCKQUOTE>
<P>
<P>
  <HR>
<ADDRESS>
  <A HREF=""../../People/Connolly/"">Dan Connolly</A><BR>
  <SMALL>$Revision: 1.11 $ $Author: dsr $<BR>
  $Date: 2000/01/31 19:30:15 $</SMALL>
</ADDRESS>
</BODY></HTML>
";
            string html3 = @"<!-- <!DOCTYPE HTML><html><head></head></html> -->";
            var res = HtmlTokenizer.Tokenize(html2);
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

            string Result = Function.MakeHtml(doc);

            Assert.AreEqual(true, true);
        }
#endif
    }
}
