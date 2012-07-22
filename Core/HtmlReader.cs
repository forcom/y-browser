using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    /// <summary>
    /// Tokenize a html document
    /// </summary>
#if DEBUG
    public
#endif
 class HtmlTokenizer
    {
#if DEBUG
        public
#endif
 enum TokenType
        {
            Text,
            Tag,
            String,
            Comment
        }

        static bool Initialized = false;

        static Regex MatchEntity = new Regex(@"(?<special>&(?<name>\w+);)|(?<special>&(?<name>\w+))|(?<numeric>&\#(?<code>[0-9]+);)|(?<numeric>&\#(?<code>[0-9]+))");
        static Regex MatchAttribute = new Regex(@"(?<tag>^[a-zA-Z0-9\.\-]*)|(?<attribute>(?<name>\w+)\s*(=\s*(((?<OpenSQ>\')(?<value>[^\']*)(?<CloseSQ-OpenSQ>\'))|((?<OpenDQ>"")(?<value>[^""]*)(?<CloseDQ-OpenDQ>""))|(?<value>[^\'""\s]+)))?)");

        /// <summary>
        /// Check if a character can be used as a tag name.
        /// </summary>
        /// <param name="c">A character to check</param>
        /// <returns>True if can be used, otherwise false.</returns>
#if DEBUG
        public
#endif
 static bool IsTagStartPossibleCharacter(char c)
        {
            return ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') || ('0' <= c && c <= '9') || c == '.' || c == '-';
        }

        /// <summary>
        /// Substitute data characters to original character.
        /// </summary>
        /// <param name="str">A string contains data characters</param>
        /// <returns>Replaced string.</returns>
#if DEBUG
        public
#endif
 static string SubstituteSpecialChar(string str)
        {
            str = MatchEntity.Replace(str, new MatchEvaluator(x =>
            {
                if (x.Groups["special"].Success)
                {
                    return HtmlTag.EntityMap.ContainsKey(x.Groups["name"].Value) ? HtmlTag.EntityMap[x.Groups["name"].Value] : x.Value;
                }
                else if (x.Groups["numeric"].Success)
                {
                    return x.Groups["code"].Success ? ((char)Int32.Parse(x.Groups["code"].Value)).ToString() : x.Value;
                }
                return x.Value;
            }));
            return str;
        }

        /// <summary>
        /// Get an Element from a tag or text string.
        /// </summary>
        /// <param name="type">Type of a string</param>
        /// <param name="str">A tag or text string</param>
        /// <returns>An Element</returns>
#if DEBUG
        public
#endif
 static Element GetElement(TokenType type, string str)
        {
            string orig_str = str;
            Element elem = null;
            switch (type)
            {
                case TokenType.Tag:
                    MatchCollection res = null;
                    switch (str[1])
                    {
                        case '!':
                            // Remove <! and >
                            str = str.Substring(2, str.Length - 3);
                            res = MatchAttribute.Matches(str);
                            foreach (Match i in res)
                            {
                                if (i.Groups["tag"].Success)
                                {
                                    elem = new Element(i.Groups["tag"].Value, Element.ElementType.Special, source: orig_str);
                                    elem.Attributes[str.Substring(i.Index + i.Length)] = "";
                                    break;
                                }
                            }
                            break;
                        case '/':
                            // Remove </ and >
                            str = str.Substring(2, str.Length - 3);
                            res = MatchAttribute.Matches(str);
                            elem = new Element("", Element.ElementType.Unknown, false, orig_str);
                            foreach (Match i in res)
                            {
                                if (i.Groups["tag"].Success)
                                {
                                    elem.Name = i.Groups["tag"].Value;
                                    if (HtmlTag.TagMap.ContainsKey(i.Groups["tag"].Value))
                                    {
                                        elem.Type = HtmlTag.TagMap[elem.Name].Type;
                                    }
                                }
                                else if (i.Groups["attribute"].Success)
                                {
                                    if (i.Groups["name"].Success && elem.Type != Element.ElementType.Unknown && HtmlTag.TagMap[elem.Name][i.Groups["name"].Value] != null)
                                    {
                                        elem.Attributes[i.Groups["name"].Value] = i.Groups["value"].Success ? SubstituteSpecialChar(i.Groups["value"].Value) : "";
                                    }
                                }
                            }
                            if (elem.Name == "") elem = null;
                            break;
                        default:
                            // Remove < and >
                            str = str.Substring(1, str.Length - 2);
                            res = MatchAttribute.Matches(str);
                            elem = new Element("", Element.ElementType.Unknown, source: orig_str);
                            foreach (Match i in res)
                            {
                                if (i.Groups["tag"].Success)
                                {
                                    elem.Name = i.Groups["tag"].Value;
                                    if (HtmlTag.TagMap.ContainsKey(i.Groups["tag"].Value))
                                    {
                                        elem.Type = HtmlTag.TagMap[elem.Name].Type;
                                    }
                                }
                                else if (i.Groups["attribute"].Success)
                                {
                                    if (i.Groups["name"].Success && elem.Type != Element.ElementType.Unknown && HtmlTag.TagMap[elem.Name][i.Groups["name"].Value] != null)
                                    {
                                        elem.Attributes[i.Groups["name"].Value] = i.Groups["value"].Success ? SubstituteSpecialChar(i.Groups["value"].Value) : "";
                                    }
                                }
                            }
                            if (elem.Name == "") elem = null;
                            break;
                    }
                    break;
                case TokenType.Text:
                    str = SubstituteSpecialChar(str);
                    elem = new Element(str, Element.ElementType.Text, source: orig_str);
                    break;
                case TokenType.Comment:
                    str = str.Substring(4, str.Length - 7);
                    elem = new Element("--", Element.ElementType.Special, source: orig_str);
                    elem.Attributes[str] = "";
                    break;
            }
            // If it failed to get an element, assume that it is a text.
            return elem ?? GetElement(TokenType.Text, orig_str);
        }

        /// <summary>
        /// Tokenize an html document.
        /// </summary>
        /// <param name="HtmlString">A html document string</param>
        /// <returns>Tokenized html document</returns>
        public static Document Tokenize(string HtmlString)
        {
            HtmlTag.Initialize();

            TokenType prevtype = TokenType.Text;
            Document doc = new Document();
            TokenType type = TokenType.Text;
            bool quote = false; //True:" , False:'
            int sp = 0;
            for (int pos = 0; pos < HtmlString.Length; ++pos)
            {
                switch (HtmlString[pos])
                {
                    case '<':
                        // If it is in a string or comment, ignore
                        if (type == TokenType.String || type == TokenType.Comment) break;
                        // If it is in a tag, actually previous one is not a tag. so change previous one as Text
                        if (type == TokenType.Tag)
                        {
                            type = TokenType.Text;
                            --pos;
                            break;
                        }
                        //If it is possible to be a tag, add previous one.
                        if (pos + 1 != HtmlString.Length && IsTagStartPossibleCharacter(HtmlString[pos + 1]))
                        {
                            if (sp <= pos - 1)
                            {
                                doc.Items.Add(GetElement(TokenType.Text, HtmlString.Substring(sp, pos - sp)));
                            }

                            type = TokenType.Tag;
                            sp = pos;
                        }
                        //If it is a comment
                        else if (pos + 4 < HtmlString.Length && HtmlString[pos + 1] == '!' && HtmlString[pos + 2] == '-' && HtmlString[pos + 3] == '-')
                        {
                            if (sp <= pos - 1)
                            {
                                doc.Items.Add(GetElement(TokenType.Text, HtmlString.Substring(sp, pos - sp)));
                            }

                            type = TokenType.Comment;
                            sp = pos;
                        }
                        // If it is possible to be an end or special tag, add previous one.
                        else if (pos + 2 != HtmlString.Length && (HtmlString[pos + 1] == '/' || HtmlString[pos + 1] == '!') && IsTagStartPossibleCharacter(HtmlString[pos + 2]))
                        {
                            if (sp <= pos - 1)
                            {
                                doc.Items.Add(GetElement(TokenType.Text, HtmlString.Substring(sp, pos - sp)));
                            }

                            type = TokenType.Tag;
                            sp = pos;
                        }
                        // Otherwise, Process as Text
                        else
                        {
                            type = TokenType.Text;
                        }
                        break;
                    case '>':
                        if (type == TokenType.Tag)
                        {
                            doc.Items.Add(GetElement(TokenType.Tag, HtmlString.Substring(sp, pos - sp + 1)));
                            sp = pos + 1;
                            type = TokenType.Text;
                        }
                        else if (type == TokenType.Comment)
                        {
                            // If it is the end of a comment
                            if (pos - 2 > 0 && HtmlString[pos - 2] == '-' && HtmlString[pos - 1] == '-')
                            {
                                doc.Items.Add(GetElement(TokenType.Comment, HtmlString.Substring(sp, pos - sp + 1)));
                                sp = pos + 1;
                                type = TokenType.Text;
                            }
                        }
                        break;
                    case '\'':
                    case '"':
                        switch (type)
                        {
                            case TokenType.String:
                                if ((quote && HtmlString[pos] == '\'') || (!quote && HtmlString[pos] == '"')) break;
                                else
                                    type = prevtype;
                                break;
                            case TokenType.Tag:
                                prevtype = type;
                                quote = (HtmlString[pos] == '"');
                                type = TokenType.String;
                                break;
                        }
                        break;
                }
            }
            if (sp != HtmlString.Length)
            {
                doc.Items.Add(GetElement(TokenType.Text, HtmlString.Substring(sp, HtmlString.Length - sp)));
            }

            return doc;
        }
    }

    /// <summary>
    /// Parse a tokenized html document.
    /// </summary>
#if DEBUG
    public
#endif
 class HtmlParser
    {
        /// <summary>
        /// Check an element can be a child.
        /// </summary>
        /// <param name="list">Parent elements</param>
        /// <param name="elem">An element to check</param>
        /// <returns>Return true if can be a child, otherwise false.</returns>
        static bool CheckCanExist(List<HtmlTag.Entity> list, Element elem)
        {
            bool flag = true;
            foreach (var i in list)
            {
                switch (elem.Type)
                {
                    case Element.ElementType.Markup:
                    case Element.ElementType.Object:
                    case Element.ElementType.Structure:
                        if (!i.Children.Contains(HtmlTag.TagMap[elem.Name]))
                        {
                            flag = false;
                            return false;
                        }
                        break;
                    case Element.ElementType.Text:
                        if (!i.Children.Contains(HtmlTag.TagMap["CDATA"]))
                        {
                            flag = false;
                            return false;
                        }
                        break;
                    case Element.ElementType.Unknown:
                    case Element.ElementType.Special:
                        flag = false;
                        return false;
                }
            }
            return flag;
        }

        /// <summary>
        /// Parse a tokenized html document.
        /// </summary>
        /// <param name="html">A tokenized html document</param>
        /// <returns>Parsed html document</returns>
        public static Document ParseHtml(Document html)
        {
            Document doc = new Document();
            HtmlTag.Initialize();

            Stack<Element> stk = new Stack<Element>();
            //List<HtmlTag.Entity> markup = new List<HtmlTag.Entity>();
            Stack<HtmlTag.Entity> markup = new Stack<HtmlTag.Entity>();
            Element top = null;

            foreach (Element i in html.Items)
            {
                top = stk.Count > 0 ? stk.Peek() : null;
                switch (i.Type)
                {
                    case Element.ElementType.Special:
                        break;
                    case Element.ElementType.Structure:
                        if (i.IsStartTag)
                        {
                            if (top != null && !HtmlTag.TagMap[top.Name].Children.Contains(HtmlTag.TagMap[i.Name]))
                                break;

                            if (HtmlTag.TagMap[i.Name].IsStartTag)
                                stk.Push(i);
                            doc.Items.Add(i);
                        }
                        else
                        {
                            if (!HtmlTag.TagMap[i.Name].IsStartTag)
                                break;

                            if (top != null && top.Name == i.Name)
                            {
                                stk.Pop();
                                doc.Items.Add(i);
                            }
                        }
                        break;
                    case Element.ElementType.Markup:
                        if (i.IsStartTag)
                        {
                            if (top != null && !HtmlTag.TagMap[top.Name].Children.Contains(HtmlTag.TagMap[i.Name]))
                                break;

                            /*if (CheckCanExist(markup, i))
                            {
                                markup.Add(HtmlTag.TagMap[i.Name]);
                                doc.Items.Add(i);
                            }*/
                            markup.Push(HtmlTag.TagMap[i.Name]);
                            doc.Items.Add(i);
                        }
                        else
                        {
                            /*if (markup.Contains(HtmlTag.TagMap[i.Name]))
                            {
                                markup.Reverse();
                                markup.Remove(HtmlTag.TagMap[i.Name]);
                                markup.Reverse();
                                doc.Items.Add(i);
                            }*/
                            if (markup.Peek() == HtmlTag.TagMap[i.Name])
                            {
                                markup.Pop();
                                doc.Items.Add(i);
                            }
                        }
                        break;
                    case Element.ElementType.Object:
                        if (top != null && !HtmlTag.TagMap[top.Name].Children.Contains(HtmlTag.TagMap[i.Name]))
                            break;
                        doc.Items.Add(i);
                        break;
                    case Element.ElementType.Text:
                        if (top != null && !HtmlTag.TagMap[top.Name].Children.Contains(HtmlTag.TagMap["CDATA"]))
                            break;
                        doc.Items.Add(i);
                        break;
                    case Element.ElementType.Unknown:
                        break;
                }
            }

            //Auto Recovery for unmarked elements =_=;;
            /*markup.Reverse();
            foreach (var i in markup)
            {
                doc.Items.Add(new Element(i.Name, Element.ElementType.Markup, false));
            }*/
            for (var i = markup.Peek(); markup.Count > 0; markup.Pop(), i = markup.Peek())
            {
                doc.Items.Add(new Element(i.Name, Element.ElementType.Markup, false));
            }

            while (stk.Count != 0)
            {
                Element i = stk.Pop();
                doc.Items.Add(new Element(i.Name, i.Type, false));
            }

            return doc;
        }
    }

    /// <summary>
    /// Get a html document from a html string.
    /// </summary>
    public class HtmlReader
    {
        /// <summary>
        /// Get a html document from a html string.
        /// </summary>
        /// <param name="html">A html string</param>
        /// <returns>A html document</returns>
        public static Document GetDocument(string html)
        {
            return HtmlParser.ParseHtml(HtmlTokenizer.Tokenize(html));
        }

        /// <summary>
        /// Get a html document from a url.
        /// </summary>
        /// <param name="url">A url to get a html string</param>
        /// <returns>A html document if the work was completed successfully, otherwise null</returns>
        public static Document GetDocumentFromURL(string url)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            string html = null;
            try
            {
                html = wc.DownloadString(url);
            }
            catch
            {
                html = null;
            }
            return html != null ? HtmlParser.ParseHtml(HtmlTokenizer.Tokenize(html)) : null;
        }
    }

    /// <summary>
    /// Information About Html Tags.
    /// </summary>
    public class HtmlTag
    {
        /// <summary>
        /// List Tags
        /// </summary>
        public enum ListType
        {
            OL,
            UL,
            DIR,
            MENU,
            DL
        }

        /// <summary>
        /// Element for structure.
        /// </summary>
        public class Entity : Element
        {
            //public Element Element { get; set; }
            List<Entity> _Children = new List<Entity>();
            public List<Entity> Children { get { return _Children; } }

            public Entity(Element element)
            {
                Name = element.Name;
                Type = element.Type;
                IsStartTag = element.IsStartTag;
                Source = element.Source;
            }
        }

        /// <summary>
        /// Get a hierarchic tag map.
        /// </summary>
        public static Dictionary<string, Entity> TagMap { get; private set; }
        /// <summary>
        /// Get a character entity map.
        /// </summary>
        public static Dictionary<string, string> EntityMap { get; private set; }
        //public static Dictionary<string, Element.ElementType> ElementMap { get; private set; }
        //public static Dictionary<string, HashSet<string>> AttributeMap { get; private set; }
        static bool _Initialized = false;
        public static bool Initialized { get { return _Initialized; } set { _Initialized = value; } }

        /// <summary>
        /// Initialize EntityMap
        /// </summary>
#if DEBUG
        public
#endif
 static void InitializeEntityMap()
        {
            EntityMap.Add("lt", "<");
            EntityMap.Add("gt", ">");
            EntityMap.Add("amp", "&");
            EntityMap.Add("quot", "\"");
        }

        /// <summary>
        /// Add all elements into a new list.
        /// </summary>
        /// <param name="Destination">A new list</param>
        /// <param name="Source">Elements</param>
        static void AddToList(List<Entity> Destination, List<Entity> Source)
        {
            for (int i = 0; i < Source.Count; ++i)
            {
                if (!Destination.Contains(Source[i]))
                    Destination.Add(Source[i]);
            }
        }

        /// <summary>
        /// Add all elements as children.
        /// </summary>
        /// <param name="Destination">Parent elements</param>
        /// <param name="Source">Children elements</param>
        static void AddToChild(List<Entity> Destination, List<Entity> Source)
        {
            for (int i = 0; i < Destination.Count; ++i)
            {
                AddToList(Destination[i].Children, Source);
            }
        }

        /// <summary>
        /// Initialize TagMap.
        /// </summary>
#if DEBUG
        public
#endif
 static void InitializeTagMap()
        {
            Entity H1 = new Entity(new Element("H1"));
            Entity H2 = new Entity(new Element("H2"));
            Entity H3 = new Entity(new Element("H3"));
            Entity H4 = new Entity(new Element("H4"));
            Entity H5 = new Entity(new Element("H5"));
            Entity H6 = new Entity(new Element("H6"));
            List<Entity> heading = new List<Entity>();
            Entity UL = new Entity(new Element("UL"));
            Entity OL = new Entity(new Element("OL"));
            Entity DIR = new Entity(new Element("DIR"));
            Entity MENU = new Entity(new Element("MENU"));
            List<Entity> list = new List<Entity>();
            Entity CDATA = new Entity(new Element("", Element.ElementType.Text, false));
            Entity TT = new Entity(new Element("TT", Element.ElementType.Markup));
            Entity B = new Entity(new Element("B", Element.ElementType.Markup));
            Entity I = new Entity(new Element("I", Element.ElementType.Markup));
            List<Entity> font = new List<Entity>();
            Entity EM = new Entity(new Element("EM", Element.ElementType.Markup));
            Entity STRONG = new Entity(new Element("STRONG", Element.ElementType.Markup));
            Entity CODE = new Entity(new Element("CODE", Element.ElementType.Markup));
            Entity SAMP = new Entity(new Element("SAMP", Element.ElementType.Markup));
            Entity KBD = new Entity(new Element("KBD", Element.ElementType.Markup));
            Entity VAR = new Entity(new Element("VAR", Element.ElementType.Markup));
            Entity CITE = new Entity(new Element("CITE", Element.ElementType.Markup));
            List<Entity> phrase = new List<Entity>();
            Entity A = new Entity(new Element("A", Element.ElementType.Markup));
            Entity IMG = new Entity(new Element("IMG", Element.ElementType.Object, false));
            Entity BR = new Entity(new Element("BR", Element.ElementType.Object, false));
            List<Entity> text = new List<Entity>();
            Entity P = new Entity(new Element("P", starttag: false));
            Entity HR = new Entity(new Element("HR", Element.ElementType.Object, false));
            Entity BLOCKQUOTE = new Entity(new Element("BLOCKQUOTE"));
            Entity FORM = new Entity(new Element("FORM"));
            Entity ISINDEX = new Entity(new Element("ISINDEX", starttag: false));
            List<Entity> block_forms = new List<Entity>();
            Entity PRE = new Entity(new Element("PRE"));
            List<Entity> preformatted = new List<Entity>();
            List<Entity> block = new List<Entity>();
            List<Entity> flow = new List<Entity>();
            List<Entity> pre_content = new List<Entity>();
            Entity DL = new Entity(new Element("DL"));
            Entity DT = new Entity(new Element("DT", starttag: false));
            Entity DD = new Entity(new Element("DD", starttag: false));
            Entity LI = new Entity(new Element("LI", starttag: false));
            List<Entity> body_content = new List<Entity>();
            Entity BODY = new Entity(new Element("BODY"));
            Entity ADDRESS = new Entity(new Element("ADDRESS"));
            Entity INPUT = new Entity(new Element("INPUT", Element.ElementType.Object, false));
            Entity SELECT = new Entity(new Element("SELECT", Element.ElementType.Object));
            Entity TEXTAREA = new Entity(new Element("TEXTAREA", Element.ElementType.Object));
            Entity OPTION = new Entity(new Element("OPTION", Element.ElementType.Object, false));
            List<Entity> head_extra = new List<Entity>();
            List<Entity> head_content = new List<Entity>();
            Entity HEAD = new Entity(new Element("HEAD"));
            Entity TITLE = new Entity(new Element("TITLE"));
            Entity LINK = new Entity(new Element("LINK", starttag: false));
            Entity BASE = new Entity(new Element("BASE", starttag: false));
            Entity NEXTID = new Entity(new Element("NEXTID", starttag: false));
            Entity META = new Entity(new Element("META", starttag: false));
            List<Entity> html_content = new List<Entity>();
            Entity HTML = new Entity(new Element("HTML"));

            heading.Add(H1);
            heading.Add(H2);
            heading.Add(H3);
            heading.Add(H4);
            heading.Add(H5);
            heading.Add(H6);

            list.Add(UL);
            list.Add(OL);
            list.Add(DIR);
            list.Add(MENU);

            font.Add(TT);
            font.Add(B);
            font.Add(I);

            phrase.Add(EM);
            phrase.Add(STRONG);
            phrase.Add(CODE);
            phrase.Add(SAMP);
            phrase.Add(KBD);
            phrase.Add(VAR);
            phrase.Add(CITE);

            text.Add(CDATA);
            text.Add(A);
            text.Add(IMG);
            text.Add(BR);
            AddToList(text, font);
            AddToList(text, phrase);

            AddToChild(font, text);
            AddToChild(phrase, text);

            AddToList(A.Children, heading);
            AddToList(A.Children, text);
            A.Children.Remove(A);

            AddToList(P.Children, text);

            AddToChild(heading, text);

            block_forms.Add(BLOCKQUOTE);
            block_forms.Add(FORM);
            block_forms.Add(ISINDEX);

            preformatted.Add(PRE);

            block.Add(P);
            AddToList(block, list);
            block.Add(DL);
            AddToList(block, preformatted);
            AddToList(block, block_forms);

            AddToList(flow, text);
            AddToList(flow, block);

            pre_content.Add(CDATA);
            pre_content.Add(A);
            pre_content.Add(HR);
            pre_content.Add(BR);

            AddToList(PRE.Children, pre_content);

            DL.Children.Add(DT);
            DL.Children.Add(DD);

            // ...=_=;;...
            AddToList(DL.Children, flow);
            // ...T_T...

            AddToList(DT.Children, text);

            AddToList(DD.Children, flow);

            OL.Children.Add(LI);
            UL.Children.Add(LI);

            DIR.Children.Add(LI);
            MENU.Children.Add(LI);

            // ...=_=;;...
            AddToList(OL.Children, flow);
            AddToList(UL.Children, flow);
            AddToList(DIR.Children, flow);
            AddToList(MENU.Children, flow);
            // ...T_T...

            AddToList(LI.Children, flow);

            AddToList(body_content, heading);
            AddToList(body_content, text);
            AddToList(body_content, block);
            body_content.Add(HR);
            body_content.Add(ADDRESS);

            AddToList(BODY.Children, body_content);

            AddToList(BLOCKQUOTE.Children, body_content);

            AddToList(ADDRESS.Children, text);
            ADDRESS.Children.Add(P);

            AddToList(FORM.Children, body_content);
            FORM.Children.Add(INPUT);
            FORM.Children.Add(SELECT);
            FORM.Children.Add(TEXTAREA);
            FORM.Children.Remove(FORM);

            SELECT.Children.Add(OPTION);
            // ...=_=;;...
            SELECT.Children.Add(CDATA);
            // ...T_T...

            OPTION.Children.Add(CDATA);

            TEXTAREA.Children.Add(CDATA);

            head_extra.Add(NEXTID);

            head_content.Add(TITLE);
            head_content.Add(ISINDEX);
            head_content.Add(BASE);
            AddToList(head_content, head_extra);

            AddToList(HEAD.Children, head_content);
            HEAD.Children.Add(META);
            HEAD.Children.Add(LINK);

            TITLE.Children.Add(CDATA);

            html_content.Add(HEAD);
            html_content.Add(BODY);

            AddToList(HTML.Children, html_content);

            TagMap.Add("CDATA", CDATA);
            TagMap.Add("H1", H1);
            TagMap.Add("H2", H2);
            TagMap.Add("H3", H3);
            TagMap.Add("H4", H4);
            TagMap.Add("H5", H5);
            TagMap.Add("H6", H6);
            TagMap.Add("UL", UL);
            TagMap.Add("OL", OL);
            TagMap.Add("DIR", DIR);
            TagMap.Add("MENU", MENU);
            TagMap.Add("TT", TT);
            TagMap.Add("B", B);
            TagMap.Add("I", I);
            TagMap.Add("EM", EM);
            TagMap.Add("STRONG", STRONG);
            TagMap.Add("CODE", CODE);
            TagMap.Add("SAMP", SAMP);
            TagMap.Add("KBD", KBD);
            TagMap.Add("VAR", VAR);
            TagMap.Add("CITE", CITE);
            TagMap.Add("A", A);
            TagMap.Add("IMG", IMG);
            TagMap.Add("BR", BR);
            TagMap.Add("HR", HR);
            TagMap.Add("P", P);
            TagMap.Add("BLOCKQUOTE", BLOCKQUOTE);
            TagMap.Add("FORM", FORM);
            TagMap.Add("ISINDEX", ISINDEX);
            TagMap.Add("PRE", PRE);
            TagMap.Add("DL", DL);
            TagMap.Add("DT", DT);
            TagMap.Add("DD", DD);
            TagMap.Add("LI", LI);
            TagMap.Add("ADDRESS", ADDRESS);
            TagMap.Add("INPUT", INPUT);
            TagMap.Add("SELECT", SELECT);
            TagMap.Add("TEXTAREA", TEXTAREA);
            TagMap.Add("OPTION", OPTION);
            TagMap.Add("NEXTID", NEXTID);
            TagMap.Add("TITLE", TITLE);
            TagMap.Add("BASE", BASE);
            TagMap.Add("META", META);
            TagMap.Add("LINK", LINK);
            TagMap.Add("HEAD", HEAD);
            TagMap.Add("BODY", BODY);
            TagMap.Add("HTML", HTML);
        }

        /// <summary>
        /// Initialize Attribute Map.
        /// </summary>
#if DEBUG
        public
#endif
 static void InitializeAttributeMap()
        {
            TagMap["HTML"]["VERSION"] = "";
            TagMap["BASE"]["HREF"] = "";
            TagMap["LINK"]["HREF"] = "";
            TagMap["LINK"]["TITLE"] = "";
            TagMap["LINK"]["REL"] = "";
            TagMap["LINK"]["REV"] = "";
            TagMap["LINK"]["URN"] = "";
            TagMap["LINK"]["METHODS"] = "";
            TagMap["META"]["HTTP-EQUIV"] = "";
            TagMap["META"]["NAME"] = "";
            TagMap["META"]["CONTENT"] = "";
            TagMap["NEXTID"]["N"] = "";
            TagMap["PRE"]["WIDTH"] = "";
            TagMap["DL"]["COMPACT"] = "";
            TagMap["A"]["HREF"] = "";
            TagMap["A"]["NAME"] = "";
            TagMap["A"]["TITLE"] = "";
            TagMap["A"]["REL"] = "";
            TagMap["A"]["REV"] = "";
            TagMap["A"]["URN"] = "";
            TagMap["A"]["METHODS"] = "";
            TagMap["IMG"]["ALIGN"] = "";
            TagMap["IMG"]["ALT"] = "";
            TagMap["IMG"]["ISMAP"] = "";
            TagMap["IMG"]["SRC"] = "";
            TagMap["FORM"]["ACTION"] = "";
            TagMap["FORM"]["METHOD"] = "";
            TagMap["FORM"]["ENCTYPE"] = "";
            TagMap["INPUT"]["TYPE"] = "";
            //INPUT TYPE=TEXT
            //INPUT TYPE=PASSWORD
            TagMap["INPUT"]["NAME"] = "";
            TagMap["INPUT"]["MAXLENGTH"] = "";
            TagMap["INPUT"]["SIZE"] = "";
            TagMap["INPUT"]["VALUE"] = "";
            //INPUT TYPE=CHECKBOX
            //INPUT TYPE=RADIO
            TagMap["INPUT"]["NAME"] = "";
            TagMap["INPUT"]["VALUE"] = "";
            TagMap["INPUT"]["CHECKED"] = "";
            //INPUT TYPE=IMAGE
            TagMap["INPUT"]["NAME"] = "";
            TagMap["INPUT"]["SRC"] = "";
            TagMap["INPUT"]["ALIGN"] = "";
            //INPUT TYPE=HIDDEN
            //INPUT TYPE=SUBMIT
            TagMap["INPUT"]["NAME"] = "";
            TagMap["INPUT"]["VALUE"] = "";
            //INPUT TYPE=RESET
            TagMap["INPUT"]["VALUE"] = "";
            TagMap["SELECT"]["MULTIPLE"] = "";
            TagMap["SELECT"]["NAME"] = "";
            TagMap["SELECT"]["SIZE"] = "";
            TagMap["OPTION"]["SELECTED"] = "";
            TagMap["OPTION"]["VALUE"] = "";
            TagMap["TEXTAREA"]["COLS"] = "";
            TagMap["TEXTAREA"]["NAME"] = "";
            TagMap["TEXTAREA"]["ROWS"] = "";
        }

        /// <summary>
        /// Initialize Maps of HTML 2.0 Elements
        /// </summary>
        public static void Initialize()
        {
            if (Initialized)
                return;

            TagMap = new Dictionary<string, Entity>(StringComparer.OrdinalIgnoreCase);
            EntityMap = new Dictionary<string, string>();

            InitializeEntityMap();
            InitializeTagMap();
            InitializeAttributeMap();

            Initialized = true;
        }
    }
}
