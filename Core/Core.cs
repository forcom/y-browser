using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    /// <summary>
    /// Represents an html attribute.
    /// </summary>
    public class Attribute
    {
        /// <summary>
        /// Name of the attribute.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Value of the attribute.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// An attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public Attribute(string name, string value = "")
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    /// Represents a collection of attributes.
    /// </summary>
    public class AttributeCollection
    {
        Dictionary<string, Attribute> _Items = new Dictionary<string, Attribute>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Attribute> Items { get { return _Items; } }
        /// <summary>
        /// Get or set of attributes in the collection
        /// </summary>
        /// <param name="key">A attribute name</param>
        /// <returns>An attribute</returns>
        public string this[string name]
        {
            get
            {
                return _Items.ContainsKey(name) ? _Items[name].Value : null;
            }
            set
            {
                _Items[name] = new Attribute(name, value);
            }
        }
    }

    /// <summary>
    /// Represents an html element.
    /// </summary>
    public class Element
    {
        /// <summary>
        /// Represents a type of element.
        /// </summary>
        public enum ElementType
        {
            Unknown,
            Special,
            Structure,
            Markup,
            Object,
            Text
        }

        /// <summary>
        /// Name of the element
        /// </summary>
        public string Name { get; set; }
        AttributeCollection _Attributes = new AttributeCollection();
        /// <summary>
        /// Attributes in the element
        /// </summary>
        public AttributeCollection Attributes { get { return _Attributes; } }
        /// <summary>
        /// Check the element is a start tag.
        /// </summary>
        public bool IsStartTag { get; set; }
        /// <summary>
        /// Type of the element
        /// </summary>
        public ElementType Type { get; set; }
        /// <summary>
        /// Original source code of the element
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Get or set an attribute value in the element
        /// </summary>
        /// <param name="attribute">Attribute Name</param>
        /// <returns>The attribute value</returns>
        public string this[string attribute]
        {
            get
            {
                return Attributes[attribute];
            }
            set
            {
                Attributes[attribute] = value;
            }
        }

        public override string ToString()
        {
            return Type + ": " + Name;
        }

        /// <summary>
        /// An element
        /// </summary>
        /// <param name="name">Element name</param>
        /// <param name="type">Element type</param>
        /// <param name="starttag">Tag type</param>
        /// <param name="source">Original source code</param>
        public Element(string name, ElementType type = ElementType.Structure, bool starttag = true, string source = "")
        {
            Name = name;
            Type = type;
            IsStartTag = starttag;
            Source = source;
        }

        protected Element()
        {
            Name = "";
            Type = ElementType.Unknown;
            IsStartTag = true;
            Source = "";
        }
    }

    /// <summary>
    /// Represents a html document.
    /// </summary>
    public class Document
    {
        List<Element> _Items = new List<Element>();
        /// <summary>
        /// A collection of elements.
        /// </summary>
        public List<Element> Items { get { return _Items; } }

        /// <summary>
        /// Get the elements by the type
        /// </summary>
        /// <param name="element">Element type</param>
        /// <returns>Elements with the type</returns>
        public List<Element> GetByElement(string element)
        {
            return Items.FindAll(x => x.Type != Element.ElementType.Text && x.Name == element);
        }

        /// <summary>
        /// Get the elements by name
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <returns>Elements with the given name</returns>
        public List<Element> this[string name]
        {
            get
            {
                return Items.FindAll(x => x.Attributes["name"] == name);
            }
        }
    }

    public class Function
    {
        public static string MakeHtml(Document doc)
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
    }
}
