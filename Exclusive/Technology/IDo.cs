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
    public interface IDo
    {
        string FindSame(string path, string search);
        string FindLike(string path, string search);
        string FindAny(string path, string search);
        string FindPattern(string path, string search);

        string FindSame(string path, Search searcher);
        string FindLike(string path, Search searcher);
        string FindAny(string path, Search searcher);
        string FindPattern(string path, Search searcher);

        string FindSame(Search searcher, string inText);
        string FindLike(Search searcher, string inText);
        string FindAny(Search searcher, string inText);
        string FindPattern(Search searcher, string inText);

        string ReplaceSame(string path, string search, string replace);
        string ReplaceLike(string path, string search, string replace);
        string ReplaceAny(string path, string search, string replace);
        string ReplacePattern(string path, string search, string replace);

        string ReplaceSame(string path, Search searcher, string replace);
        string ReplaceLike(string path, Search searcher, string replace);
        string ReplaceAny(string path, Search searcher, string replace);
        string ReplacePattern(string path, Search searcher, string replace);

        string ReplaceSame(Search searcher, string replace, string inText);
        string ReplaceLike(Search searcher, string replace, string inText);
        string ReplaceAny(Search searcher, string replace, string inText);
        string ReplacePattern(Search searcher, string replace, string inText);
    }
}
