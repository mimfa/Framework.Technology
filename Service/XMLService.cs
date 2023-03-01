using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MiMFa.Service
{
    public class XMLService
    {
        public static bool SetElement(XmlElement elem, string name, string value)
        {
            if (value == null) return false;
            var tag = elem.GetElementsByTagName(name);
            if (tag != null && tag.Count > 0)
            {
                tag[0].InnerText = value;
                return true;
            }
            return false;
        }
        public static bool SetOrAppendElement(XmlDocument parentDocument, XmlElement targetElement, string name, string value)
        {
            if (value == null) return false;
            if (SetElement(targetElement, name, value)) return true;
            var ch = parentDocument.CreateElement(name);
            ch.InnerText = value;
            targetElement.AppendChild(ch);
            return true;
        }
        public static bool SetOrPrependElement(XmlDocument parentDocument, XmlElement targetElement, string name, string value)
        {
            if (value == null) return false;
            if (SetElement(targetElement, name, value)) return true;
            var ch = parentDocument.CreateElement(name);
            ch.InnerText = value;
            targetElement.PrependChild(ch);
            return true;
        }
    }
}
