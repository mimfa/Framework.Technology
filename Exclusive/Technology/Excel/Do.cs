using iTextSharp.text.pdf;
using MiMFa.Engine;
using MiMFa.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiMFa.Exclusive.Technology.Excel
{
    public class Do : Exclusive.Technology.Do
    {
        public Do()
        {
            Converter = new  Convert();
        }

        public override string FindSame(string path, Search searcher)
        {
            foreach (var item in ((Convert)Converter).ToLines(path))
                if (searcher.FindSameIn(item)) return item;
            return null;
        }
        public override string FindLike(string path, Search searcher)
        {
            foreach (var item in ((Convert)Converter).ToLines(path))
                if (searcher.FindLikeIn(item)) return item;
            return null;
        }
        public override string FindAny(string path, Search searcher)
        {
            foreach (var item in ((Convert)Converter).ToLines(path))
                if (searcher.FindAnyIn(item)) return item;
            return null;
        }
        public override string FindPattern(string path, Search searcher)
        {
            foreach (var item in ((Convert)Converter).ToLines(path))
                if (searcher.FindPatternIn(item)) return item;
            return null;
        }


        public override string ReplaceSame(string path, Search searcher, string replace)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = default(Microsoft.Office.Interop.Excel.Workbook);

                wb = xlapp.Workbooks.Open(path);

                wb.Worksheets[1].Cells.Replace(searcher.SearchWord, replace);

                wb.Save();
                return "";
            }
            catch { return null; }
            finally { }
        }
        public override string ReplaceLike(string path, Search searcher, string replace)
        {
            return ReplaceSame(path, searcher, replace);
        }
        public override string ReplaceAny(string path, Search searcher, string replace)
        {
            return ReplaceSame(path, searcher, replace);
        }
        public override string ReplacePattern(string path, Search searcher, string replace)
        {
            return ReplacePattern(path, searcher, replace);
        }

    }
}
