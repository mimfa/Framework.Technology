using iTextSharp.text.pdf;
using MiMFa.Engine;
using MiMFa.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiMFa.Exclusive.Technology
{
    public class Do : IDo
    {
        public virtual Convert Converter { get; set; } = new Convert();


        public virtual string FindSame(string path, string search)
        {
            return FindSame(path, new Search(search));
        }
        public virtual string FindLike(string path, string search)
        {
            return FindLike(path, new Search(search));
        }
        public virtual string FindAny(string path, string search)
        {
            return FindAny(path, new Search(search));
        }
        public virtual string FindPattern(string path, string search)
        {
            return FindPattern(path, new Search(search));
        }

        public virtual string FindSame(string path, Search searcher)
        {
            foreach (var item in Converter.ToLines(path))
                if (searcher.FindSameIn(item)) return item;
            return null;
        }
        public virtual string FindLike(string path, Search searcher)
        {
            foreach (var item in Converter.ToLines(path))
                if (searcher.FindLikeIn(item)) return item;
            return null;
        }
        public virtual string FindAny(string path, Search searcher)
        {
            foreach (var item in Converter.ToLines(path))
                if (searcher.FindAnyIn(item)) return item;
            return null;
        }
        public virtual string FindPattern(string path, Search searcher)
        {
            foreach (var item in Converter.ToLines(path))
                if (searcher.FindPatternIn(item)) return item;
            return null;
        }

        public virtual string FindSame(Search searcher, string inText)
        {
            return searcher.FindSameIn(inText)? inText:null;
        }
        public virtual string FindLike(Search searcher, string inText)
        {
            return searcher.FindLikeIn(inText)? inText:null;
        }
        public virtual string FindAny(Search searcher, string inText)
        {
            return searcher.FindAnyIn(inText)? inText:null;
        }
        public virtual string FindPattern(Search searcher, string inText)
        {
            return searcher.FindPatternIn(inText)? inText:null;
        }

        public virtual string ReplaceSame(string path, string search, string replace)
        {
            return ReplaceSame(path, new Search(search), replace);
        }
        public virtual string ReplaceLike(string path, string search, string replace)
        {
            return ReplaceLike(path, new Search(search), replace);
        }
        public virtual string ReplaceAny(string path, string search, string replace)
        {
            return ReplaceAny(path, new Search(search), replace);
        }
        public virtual string ReplacePattern(string path, string search, string replace)
        {
            return ReplacePattern(path, new Search(search), replace);
        }

        public virtual string ReplaceSame(string path, Search searcher, string replace)
        {
            string res = null;
            Converter.ToFile(path, res = ReplaceSame(searcher, replace, Converter.ToString(path)));
            return res;
        }
        public virtual string ReplaceLike(string path, Search searcher, string replace)
        {
            string res = null;
            Converter.ToFile(path, res = ReplaceLike(searcher, replace, Converter.ToString(path)));
            return res;
        }
        public virtual string ReplaceAny(string path, Search searcher, string replace)
        {
            string res = null;
            Converter.ToFile(path, res = ReplaceAny(searcher, replace, Converter.ToString(path)));
            return res;
        }
        public virtual string ReplacePattern(string path, Search searcher, string replace)
        {
            string res = null;
            Converter.ToFile(path, res = ReplacePattern(searcher, replace, Converter.ToString(path)));
            return res;
        }

        public virtual string ReplaceSame(Search searcher, string replace, string inText)
        {
            return searcher.ReplaceSameIn(inText, replace);
        }
        public virtual string ReplaceLike(Search searcher, string replace, string inText)
        {
            return searcher.ReplaceLikeIn(inText, replace);
        }
        public virtual string ReplaceAny(Search searcher, string replace, string inText)
        {
            return searcher.ReplaceAnyIn(inText, replace);
        }
        public virtual string ReplacePattern(Search searcher, string replace, string inText)
        {
            return searcher.ReplacePatternIn(inText, replace);
        }
    }
}
