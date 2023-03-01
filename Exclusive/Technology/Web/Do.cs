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

namespace MiMFa.Exclusive.Technology.Web
{
    public class Do : Exclusive.Technology.Do
    {
        public Do()
        {
            Converter = new Convert();
        }

        public override string FindSame(string path, Search searcher)
        {
            return base.FindSame(searcher, Converter.ToText(path));
        }
        public override string FindLike(string path, Search searcher)
        {
            return base.FindLike(searcher, Converter.ToText(path));
        }
        public override string FindAny(string path, Search searcher)
        {
            return base.FindAny(searcher, Converter.ToText(path));
        }
        public override string FindPattern(string path, Search searcher)
        {
            return base.FindPattern(searcher, Converter.ToText(path));
        }
    }
}
