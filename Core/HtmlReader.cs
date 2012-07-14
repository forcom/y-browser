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

        static Dictionary<string, string> EntityMap = new Dictionary<string, string>();
        static Dictionary<string, Element.ElementType> ElementMap = new Dictionary<string, Element.ElementType>(StringComparer.OrdinalIgnoreCase);
        static Dictionary<string, HashSet<string>> AttributeMap = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        static Regex MatchEntity = new Regex(@"(?<special>&(?<name>\w+);)|(?<special>&(?<name>\w+))|(?<numeric>&\#(?<code>[0-9]+);)|(?<numeric>&\#(?<code>[0-9]+))");
        static Regex MatchAttribute = new Regex(@"(?<tag>^[a-zA-Z0-9\.\-]*)|(?<attribute>(?<name>\w+)\s*(=\s*(((?<OpenSQ>\')(?<value>[^\']*)(?<CloseSQ-OpenSQ>\'))|((?<OpenDQ>"")(?<value>[^""]*)(?<CloseDQ-OpenDQ>""))|(?<value>[^\'""\s]+)))?)");

        /// <summary>
        /// Initialize HtmlTokenizer
        /// </summary>
#if DEBUG
    public
#endif
        static void Init()
        {
            EntityMap.Add("lt", "<");
            EntityMap.Add("gt", ">");
            EntityMap.Add("amp", "&");
            EntityMap.Add("quot", "\"");

            ElementMap.Add("HTML", Element.ElementType.Structure);
            ElementMap.Add("HEAD", Element.ElementType.Structure);
            ElementMap.Add("TITLE", Element.ElementType.Structure);
            ElementMap.Add("BASE", Element.ElementType.Structure);
            ElementMap.Add("ISINDEX", Element.ElementType.Structure);
            ElementMap.Add("LINK", Element.ElementType.Structure);
            ElementMap.Add("META", Element.ElementType.Structure);
            ElementMap.Add("NEXTID", Element.ElementType.Structure);
            ElementMap.Add("BODY", Element.ElementType.Structure);
            ElementMap.Add("H1", Element.ElementType.Structure);
            ElementMap.Add("H2", Element.ElementType.Structure);
            ElementMap.Add("H3", Element.ElementType.Structure);
            ElementMap.Add("H4", Element.ElementType.Structure);
            ElementMap.Add("H5", Element.ElementType.Structure);
            ElementMap.Add("H6", Element.ElementType.Structure);
            ElementMap.Add("P", Element.ElementType.Structure);
            ElementMap.Add("PRE", Element.ElementType.Structure);
            ElementMap.Add("ADDRESS", Element.ElementType.Structure);
            ElementMap.Add("BLOCKQUOTE", Element.ElementType.Structure);
            ElementMap.Add("UL", Element.ElementType.Structure);
            ElementMap.Add("LI", Element.ElementType.Structure);
            ElementMap.Add("OL", Element.ElementType.Structure);
            ElementMap.Add("DIR", Element.ElementType.Structure);
            ElementMap.Add("MENU", Element.ElementType.Structure);
            ElementMap.Add("DL", Element.ElementType.Structure);
            ElementMap.Add("DT", Element.ElementType.Structure);
            ElementMap.Add("DD", Element.ElementType.Structure);
            ElementMap.Add("CITE", Element.ElementType.Markup);
            ElementMap.Add("CODE", Element.ElementType.Markup);
            ElementMap.Add("EM", Element.ElementType.Markup);
            ElementMap.Add("KBD", Element.ElementType.Markup);
            ElementMap.Add("SAMP", Element.ElementType.Markup);
            ElementMap.Add("STRONG", Element.ElementType.Markup);
            ElementMap.Add("VAR", Element.ElementType.Markup);
            ElementMap.Add("B", Element.ElementType.Markup);
            ElementMap.Add("I", Element.ElementType.Markup);
            ElementMap.Add("TT", Element.ElementType.Markup);
            ElementMap.Add("A", Element.ElementType.Markup);
            ElementMap.Add("BR", Element.ElementType.Object);
            ElementMap.Add("HR", Element.ElementType.Object);
            ElementMap.Add("IMG", Element.ElementType.Object);
            ElementMap.Add("FORM", Element.ElementType.Structure);
            ElementMap.Add("INPUT", Element.ElementType.Object);
            ElementMap.Add("SELECT", Element.ElementType.Object);
            ElementMap.Add("OPTION", Element.ElementType.Object);
            ElementMap.Add("TEXTAREA", Element.ElementType.Object);

            HashSet<string> set = null;

            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("VERSION");
            AttributeMap.Add("HTML", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("HEAD", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("TITLE", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("HREF");
            AttributeMap.Add("BASE", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("ISINDEX", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("HREF");
            set.Add("TITLE");
            set.Add("REL");
            set.Add("REV");
            set.Add("URN");
            set.Add("METHODS");
            AttributeMap.Add("LINK", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("HTTP-EQUIV");
            set.Add("NAME");
            set.Add("CONTENT");
            AttributeMap.Add("META", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("N");
            AttributeMap.Add("NEXTID", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("BODY", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("H1", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("H2", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("H3", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("H4", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("H5", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("H6", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("P", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("WIDTH");
            AttributeMap.Add("PRE", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("ADDRESS", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("BLOCKQUOTE", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("UL", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("LI", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("OL", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("DIR", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("MENU", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("COMPACT");
            AttributeMap.Add("DL", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("DT", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("DD", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("CITE", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("CODE", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("EM", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("KBD", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("SAMP", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("STRONG", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("VAR", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("B", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("I", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("TT", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("HREF");
            set.Add("NAME");
            set.Add("TITLE");
            set.Add("REL");
            set.Add("REV");
            set.Add("URN");
            set.Add("METHODS");
            AttributeMap.Add("A", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("BR", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AttributeMap.Add("HR", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("ALIGN");
            set.Add("ALT");
            set.Add("ISMAP");
            set.Add("SRC");
            AttributeMap.Add("IMG", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("ACTION");
            set.Add("METHOD");
            set.Add("ENCTYPE");
            AttributeMap.Add("FORM", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("TYPE");
            //INPUT TYPE=TEXT
            //INPUT TYPE=PASSWORD
            set.Add("NAME");
            set.Add("MAXLENGTH");
            set.Add("SIZE");
            set.Add("VALUE");
            //INPUT TYPE=CHECKBOX
            set.Add("NAME");
            set.Add("VALUE");
            set.Add("CHECKED");
            //INPUT TYPE=RADIO
            set.Add("NAME");
            set.Add("VALUE");
            set.Add("CHECKED");
            //INPUT TYPE=IMAGE
            set.Add("NAME");
            set.Add("SRC");
            set.Add("ALIGN");
            //INPUT TYPE=HIDDEN
            set.Add("NAME");
            set.Add("VALUE");
            //INPUT TYPE=SUBMIT
            set.Add("NAME");
            set.Add("VALUE");
            //INPUT TYPE=RESET
            set.Add("VALUE");
            AttributeMap.Add("INPUT", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("MULTIPLE");
            set.Add("NAME");
            set.Add("SIZE");
            AttributeMap.Add("SELECT", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("SELECTED");
            set.Add("VALUE");
            AttributeMap.Add("OPTION", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("COLS");
            set.Add("NAME");
            set.Add("ROWS");
            AttributeMap.Add("TEXTAREA", set);

            Initialized = true;
        }

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
            var res = MatchEntity.Matches(str);
            str = MatchEntity.Replace(str, new MatchEvaluator(x =>
            {
                if (x.Groups["special"].Success)
                {
                    return EntityMap.ContainsKey(x.Groups["name"].Value) ? EntityMap[x.Groups["name"].Value] : x.Value;
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
                case TokenType.Tag :
                    MatchCollection res = null;
                    switch (str[1])
                    {
                        case '!' :
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
                        case '/' :
                            // Remove </ and >
                            str = str.Substring(2, str.Length - 3);
                            res = MatchAttribute.Matches(str);
                            elem = new Element("", Element.ElementType.Unknown, false, orig_str);
                            foreach (Match i in res)
                            {
                                if (i.Groups["tag"].Success)
                                {
                                    elem.Name = i.Groups["tag"].Value;
                                    if (ElementMap.ContainsKey(i.Groups["tag"].Value))
                                    {
                                        elem.Type = ElementMap[elem.Name];
                                    }
                                }
                                else if (i.Groups["attribute"].Success)
                                {
                                    if (i.Groups["name"].Success && elem.Type != Element.ElementType.Unknown && AttributeMap[elem.Name].Contains(i.Groups["name"].Value))
                                    {
                                        elem.Attributes[i.Groups["name"].Value] = i.Groups["value"].Success ? SubstituteSpecialChar(i.Groups["value"].Value) : "";
                                    }
                                }
                            }
                            if (elem.Name == "") elem = null;
                            break;
                        default :
                            // Remove < and >
                            str = str.Substring(1, str.Length - 2);
                            res = MatchAttribute.Matches(str);
                            elem = new Element("", Element.ElementType.Unknown, source: orig_str);
                            foreach (Match i in res)
                            {
                                if (i.Groups["tag"].Success)
                                {
                                    elem.Name = i.Groups["tag"].Value;
                                    if (ElementMap.ContainsKey(i.Groups["tag"].Value))
                                    {
                                        elem.Type = ElementMap[elem.Name];
                                    }
                                }
                                else if (i.Groups["attribute"].Success)
                                {
                                    if (i.Groups["name"].Success && elem.Type != Element.ElementType.Unknown && AttributeMap[elem.Name].Contains(i.Groups["name"].Value))
                                    {
                                        elem.Attributes[i.Groups["name"].Value] = i.Groups["value"].Success ? SubstituteSpecialChar(i.Groups["value"].Value) : "";
                                    }
                                }
                            }
                            if (elem.Name == "") elem = null;
                            break;
                    }
                    break;
                case TokenType.Text :
                    str = SubstituteSpecialChar(str);
                    elem = new Element(str, Element.ElementType.Text, source: orig_str);
                    break;
                case TokenType.Comment :
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
            if (!Initialized)
                Init();

            TokenType prevtype = TokenType.Text;
            Document doc = new Document();
            TokenType type = TokenType.Text;
            bool quote = false; //True:" , False:'
            int sp = 0;
            for (int pos = 0; pos < HtmlString.Length; ++pos)
            {
                switch (HtmlString[pos])
                {
                    case '<' :
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
                            if ( sp <= pos - 1 )
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
                    case '>' :
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
                    case '\'' :
                    case '"' :
                        switch ( type ){
                            case TokenType.String :
                                if ((quote && HtmlString[pos] == '\'') || (!quote && HtmlString[pos] == '"')) break;
                                else
                                    type = prevtype;
                                break;
                            case  TokenType.Tag :
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

#if DEBUG
    public
#endif
    class HtmlParser
    {
        public class Entity
        {
            public Element Element { get; set; }
            List<Entity> _Children = new List<Entity>();
            public List<Entity> Children { get { return _Children; } }

            public Entity(Element element)
            {
                Element = element;
            }

            public override string ToString()
            {
                return Element.ToString();
            }
        }

        static void AddToList(List<Entity> Destination, List<Entity> Source)
        {
            for (int i = 0; i < Source.Count; ++i)
            {
                if (!Destination.Contains(Source[i]))
                    Destination.Add(Source[i]);
            }
        }

        static void AddToChild(List<Entity> Destination, List<Entity> Source)
        {
            for (int i = 0; i < Destination.Count; ++i)
            {
                AddToList(Destination[i].Children, Source);
            }
        }

        static Entity H1 = new Entity(new Element("H1"));
        static Entity H2 = new Entity(new Element("H2"));
        static Entity H3 = new Entity(new Element("H3"));
        static Entity H4 = new Entity(new Element("H4"));
        static Entity H5 = new Entity(new Element("H5"));
        static Entity H6 = new Entity(new Element("H6"));
        static List<Entity> heading = new List<Entity>();
        static Entity UL = new Entity(new Element("UL"));
        static Entity OL = new Entity(new Element("OL"));
        static Entity DIR = new Entity(new Element("DIR"));
        static Entity MENU = new Entity(new Element("MENU"));
        static List<Entity> list = new List<Entity>();
        static Entity CDATA = new Entity(new Element("", Element.ElementType.Text, false));
        static Entity TT = new Entity(new Element("TT", Element.ElementType.Markup));
        static Entity B = new Entity(new Element("B", Element.ElementType.Markup));
        static Entity I = new Entity(new Element("I", Element.ElementType.Markup));
        static List<Entity> font = new List<Entity>();
        static Entity EM = new Entity(new Element("EM", Element.ElementType.Markup));
        static Entity STRONG = new Entity(new Element("STRONG", Element.ElementType.Markup));
        static Entity CODE = new Entity(new Element("CODE", Element.ElementType.Markup));
        static Entity SAMP = new Entity(new Element("SAMP", Element.ElementType.Markup));
        static Entity KBD = new Entity(new Element("KBD", Element.ElementType.Markup));
        static Entity VAR = new Entity(new Element("VAR", Element.ElementType.Markup));
        static Entity CITE = new Entity(new Element("CITE", Element.ElementType.Markup));
        static List<Entity> phrase = new List<Entity>();
        static Entity A = new Entity(new Element("A", Element.ElementType.Markup));
        static Entity IMG = new Entity(new Element("IMG", Element.ElementType.Object, false));
        static Entity BR = new Entity(new Element("BR", Element.ElementType.Object, false));
        static List<Entity> text = new List<Entity>();
        static Entity P = new Entity(new Element("P", starttag: false));
        static Entity HR = new Entity(new Element("HR", Element.ElementType.Object, false));
        static Entity BLOCKQUOTE = new Entity(new Element("BLOCKQUOTE"));
        static Entity FORM = new Entity(new Element("FORM"));
        static Entity ISINDEX = new Entity(new Element("ISINDEX", starttag: false));
        static List<Entity> block_forms = new List<Entity>();
        static Entity PRE = new Entity(new Element("PRE"));
        static List<Entity> preformatted = new List<Entity>();
        static List<Entity> block = new List<Entity>();
        static List<Entity> flow = new List<Entity>();
        static List<Entity> pre_content = new List<Entity>();
        static Entity DL = new Entity(new Element("DL"));
        static Entity DT = new Entity(new Element("DT", starttag: false));
        static Entity DD = new Entity(new Element("DD", starttag: false));
        static Entity LI = new Entity(new Element("LI", starttag: false));
        static List<Entity> body_content = new List<Entity>();
        static Entity BODY = new Entity(new Element("BODY"));
        static Entity ADDRESS = new Entity(new Element("ADDRESS"));
        static Entity INPUT = new Entity(new Element("INPUT", Element.ElementType.Object, false));
        static Entity SELECT = new Entity(new Element("SELECT", Element.ElementType.Object));
        static Entity TEXTAREA = new Entity(new Element("TEXTAREA", Element.ElementType.Object));
        static Entity OPTION = new Entity(new Element("OPTION", Element.ElementType.Object, false));
        static List<Entity> head_extra = new List<Entity>();
        static List<Entity> head_content = new List<Entity>();
        static Entity HEAD = new Entity(new Element("HEAD"));
        static Entity TITLE = new Entity(new Element("TITLE"));
        static Entity LINK = new Entity(new Element("LINK", starttag: false));
        static Entity BASE = new Entity(new Element("BASE", starttag: false));
        static Entity NEXTID = new Entity(new Element("NEXTID", starttag: false));
        static Entity META = new Entity(new Element("META", starttag: false));
        static List<Entity> html_content = new List<Entity>();
        static Entity HTML = new Entity(new Element("HTML"));

        static Dictionary<string, Entity> TagMap = new Dictionary<string, Entity>(StringComparer.OrdinalIgnoreCase);

        static bool Initialized = false;

#if DEBUG
        public
#endif
        static void Init()
        {
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

            AddToList(DT.Children, text);

            AddToList(DD.Children, flow);

            OL.Children.Add(LI);
            UL.Children.Add(LI);

            DIR.Children.Add(LI);
            MENU.Children.Add(LI);

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

            Initialized = true;
        }

        public static void ParseHtml(string html)
        {
            if (!Initialized)
                Init();
        }
    }

    public class HtmlReader
    {
        public static Document GetDocument(string html)
        {
            Document doc = new Document();
            return doc;
        }
    }
}
