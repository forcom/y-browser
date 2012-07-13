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
            ElementMap.Add("FORM", Element.ElementType.Object);
            ElementMap.Add("INPUT", Element.ElementType.Object);
            ElementMap.Add("SELECT", Element.ElementType.Object);
            ElementMap.Add("OPTION", Element.ElementType.Object);
            ElementMap.Add("TEXTAREA", Element.ElementType.Object);

            HashSet<string> set = null;

            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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
            AttributeMap.Add("LINK", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            set.Add("HTTP-EQUIV");
            set.Add("NAME");
            set.Add("CONTENT");
            AttributeMap.Add("META", set);
            set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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
        void Init()
        {
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
