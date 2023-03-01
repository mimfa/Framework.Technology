using iTextSharp.text.pdf;
using MiMFa.Engine;
using MiMFa.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiMFa.Exclusive.Technology.Word
{
    public class Do : Exclusive.Technology.Do
    {
        public Do()
        {
            Converter = new Convert();
        }

        public override string FindSame(string path, Search searcher)
        {
            foreach (var item in ((Convert)Converter).ToContents(path))
                if (searcher.FindSameIn(item)) return item;
            return null;
        }
        public override string FindLike(string path, Search searcher)
        {
            foreach (var item in ((Convert)Converter).ToContents(path))
                if (searcher.FindLikeIn(item)) return item;
            return null;
        }
        public override string FindAny(string path, Search searcher)
        {
            foreach (var item in ((Convert)Converter).ToContents(path))
                if (searcher.FindAnyIn(item)) return item;
            return null;
        }
        public override string FindPattern(string path, Search searcher)
        {
            foreach (var item in ((Convert)Converter).ToContents(path))
                if (searcher.FindPatternIn(item)) return item;
            return null;
        }

        public override string ReplaceSame(string path, Search searcher, string replace)
        {
            XElement xmldoc = ((Convert)Converter).ToXElement(path);
            string res = null;
            Converter.ToFile(path, res = ChangeInnerText(xmldoc, (text) => searcher.ReplaceSameIn(text, replace)).ToString());
            return res;
        }
        public override string ReplaceLike(string path, Search searcher, string replace)
        {
            XElement xmldoc = ((Convert)Converter).ToXElement(path);
            string res = null;
            Converter.ToFile(path, res = ChangeInnerText(xmldoc, (text) => searcher.ReplaceLikeIn(text, replace)).ToString());
            return res;
        }
        public override string ReplaceAny(string path, Search searcher, string replace)
        {
            XElement xmldoc = ((Convert)Converter).ToXElement(path);
            string res = null;
            Converter.ToFile(path, res = ChangeInnerText(xmldoc, (text) => searcher.ReplaceAnyIn(text, replace)).ToString());
            return res;
        }
        public override string ReplacePattern(string path, Search searcher, string replace)
        {
            XElement xmldoc = ((Convert)Converter).ToXElement(path);
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
