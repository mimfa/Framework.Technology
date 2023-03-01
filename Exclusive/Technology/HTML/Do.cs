using iTextSharp.text.pdf;
using MiMFa.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using MiMFa.Engine;
using System.IO;
using System.Windows.Forms;

namespace MiMFa.Exclusive.Technology.HTML
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
            HtmlDocument doc = ((HTML.Convert)Converter).ToHtmlDocument(path);
            foreach (HtmlElement element in doc.All)
                if (!string.IsNullOrEmpty(base.FindSame(searcher, element.InnerText))) return element.InnerText;
                else if (!string.IsNullOrEmpty(base.FindSame(searcher, element.InnerHtml))) return element.InnerHtml;
            return null;
        }
        public virtual string FindLikeInAll(string path, Search searcher)
        {
            HtmlDocument doc = ((HTML.Convert)Converter).ToHtmlDocument(path);
            foreach (HtmlElement element in doc.All)
                if (!string.IsNullOrEmpty(base.FindLike(searcher, element.InnerText))) return element.InnerText;
                else if (!string.IsNullOrEmpty(base.FindLike(searcher, element.InnerHtml))) return element.InnerHtml;
            return null;
        }
        public virtual string FindAnyInAll(string path, Search searcher)
        {
            HtmlDocument doc = ((HTML.Convert)Converter).ToHtmlDocument(path);
            foreach (HtmlElement element in doc.All)
                if (element != null)
                    if (!string.IsNullOrEmpty(base.FindAny(searcher, element.InnerText))) return element.InnerText;
                    else if (!string.IsNullOrEmpty(base.FindAny(searcher, element.InnerHtml))) return element.InnerHtml;
            return null;
        }
        public virtual string FindPatternInAll(string path, Search searcher)
        {
            HtmlDocument doc = ((HTML.Convert)Converter).ToHtmlDocument(path);
            foreach (HtmlElement element in doc.All)
                if (element != null)
                    if (!string.IsNullOrEmpty(base.FindPattern(searcher, element.InnerText))) return element.InnerText;
                    else if (!string.IsNullOrEmpty(base.FindPattern(searcher, element.InnerHtml))) return element.InnerHtml;
            return null;
        }

    }
}
