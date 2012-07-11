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
        /// <summary>
        /// Get or set of attributes in the collection
        /// </summary>
        /// <param name="key">A attribute name</param>
        /// <returns>An attribute</returns>
        public string this[string name]
        {
            get
            {
                return _Items[name] != null ? _Items[name].Value : null;
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
            Special,
            Structure,
            Markup
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

        /// <summary>
        /// An element
        /// </summary>
        /// <param name="name">Element name</param>
        /// <param name="starttag">Tag type</param>
        public Element(string name, ElementType type = ElementType.Structure, bool starttag = true)
        {
            Name = name;
            Type = type;
            IsStartTag = starttag;
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
            return Items.FindAll(x => x.Name == element);
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
}
