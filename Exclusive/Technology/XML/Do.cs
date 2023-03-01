using iTextSharp.text.pdf;
using MiMFa.Engine;
using MiMFa.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MiMFa.Exclusive.Technology.XML
{
    public class Do : Exclusive.Technology.Do
    {
        public Do()
        {
            Converter = new Convert();
        }


        public virtual string FindSameInAll(string path, string search)
        {
            return FindSameInAll(path, new Search(search));
        }
        public virtual string FindLikeInAll(string path, string search)
        {
            return FindLikeInAll(path, new Search(search));
        }
        public virtual string FindAnyInAll(string path, string search)
        {
            return FindAnyInAll(path, new Search(search));
        }
        public virtual string FindPatternInAll(string path, string search)
        {
            return FindPatternInAll(path, new Search(search));
        }

        public virtual string FindSameInAll(string path, Search searcher)
        {
            XmlDocument xmldoc =  ((XML.Convert)Converter).ToXmlDocument(path);
            if (!string.IsNullOrEmpty(base.FindSame(searcher, xmldoc.InnerText))) return xmldoc.InnerText;
            return base.FindSame(searcher, xmldoc.InnerXml);
        }
        public virtual string FindLikeInAll(string path, Search searcher)
        {
            XmlDocument xmldoc = ((XML.Convert)Converter).ToXmlDocument(path);
            if (!string.IsNullOrEmpty(base.FindLike(searcher, xmldoc.InnerText))) return xmldoc.InnerText;
            return base.FindLike(searcher, xmldoc.InnerXml);
        }
        public virtual string FindAnyInAll(string path, Search searcher)
        {
            XmlDocument xmldoc = ((XML.Convert)Converter).ToXmlDocument(path);
            if (!string.IsNullOrEmpty(base.FindAny(searcher, xmldoc.InnerText))) return xmldoc.InnerText;
            return base.FindAny(searcher, xmldoc.InnerXml);
        }
        public virtual string FindPatternInAll(string path, Search searcher)
        {
            XmlDocument xmldoc = ((XML.Convert)Converter).ToXmlDocument(path);
            if (!string.IsNullOrEmpty(base.FindPattern(searcher, xmldoc.InnerText))) return xmldoc.InnerText;
            return base.FindPattern(searcher, xmldoc.InnerXml);
        }

        public override string ReplaceSame(string path, Search searcher, string replace)
        {
            XElement xmldoc = ((XML.Convert)Converter).ToXElement(path);
            string res = null;
            Converter.ToFile(path, res = ChangeInnerText(xmldoc, (text) => searcher.ReplaceSameIn(text, replace)).ToString());
            return res;
        }
        public override string ReplaceLike(string path, Search searcher, string replace)
        {
            XElement xmldoc = ((XML.Convert)Converter).ToXElement(path);
            string res = null;
            Converter.ToFile(path, res = ChangeInnerText(xmldoc, (text) => searcher.ReplaceLikeIn(text, replace)).ToString());
            return res;
        }
        public override string ReplaceAny(string path, Search searcher, string replace)
        {
            XElement xmldoc = ((XML.Convert)Converter).ToXElement(path);
            string res = null;
            Converter.ToFile(path, res = ChangeInnerText(xmldoc, (text) => searcher.ReplaceAnyIn(text, replace)).ToString());
            return res;
        }
        public override string ReplacePattern(string path, Search searcher, string replace)
        {
            XElement xmldoc = ((XML.Convert)Converter).ToXElement(path);
            string res = null;
            Converter.ToFile(path, res = ChangeInnerText(xmldoc, (text) => searcher.ReplacePatternIn(text, replace)).ToString());
            return res;
        }

        public virtual XElement ChangeInnerText(XElement xmldoc, Func<string, string> changefunc)
        {
            IEnumerable<XElement> elems = xmldoc.Elements();
            if (!elems.Any())
            {
                xmldoc.Value = changefunc(xmldoc.Value);
                return xmldoc;
            }
            foreach (XElement ele in elems)
                ele.ReplaceWith(ChangeInnerText(ele, changefunc));
            return xmldoc;
        }
    }
}
