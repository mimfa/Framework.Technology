using iTextSharp.text.pdf;
using MiMFa.Model.IO;
using MiMFa.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiMFa.Exclusive.Technology.JSON
{
    public class Convert : Exclusive.Technology.Convert
    {
        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }


        public override string ToText(string path)
        {
            return  string.Join(BreakSign, ToLines(path));
        }
        public override IEnumerable<string> ToLines(string path)
        {
            return from v in ToRows(path) select string.Join(SplitSign, v);
        }
        public override IEnumerable<IEnumerable<string>> ToRows(string path)
        {
            return ToChainedFile(path).ReadRows();
        }
        public override ChainedFile ToChainedFile(string path)
        {
            return new ChainedFile(path, Encoding) { LinesSplitters = new string[] { "},\r\n{", "},{", "},\r\n", "},\n{", "},\n", "},","}", "{" }, WarpsSplitters = new string[] { "\":\"", "\",\"", "\':\'", "\',\'", ":", ",","\"" } };
        }
        public override void ToFile(string path, ChainedFile doc)
        {
            if(doc.HasPieceColumnsLabels) ToFile(path, Pair(doc.ReadRecords(doc.ColumnsLabelsIndex+1,true)));
            else ToFile(path, doc.ReadRows());
        }
        public override void ToFile(string path, IEnumerable<IEnumerable<string>> rows)
        {
            ToFile(path, from v in rows select Collect(v));
        }
        public override void ToFile(string path, IEnumerable<string> lines)
        {
            ChainedFile cf = new ChainedFile(path, Encoding);
            cf.Clear();
            cf.WriteLine("{");
            if (lines.Any())
            {
                cf.WriteLines(from v in lines select string.Join("", v, ","));
                cf.ChangeLine(cf.PieceLinesCount - 1,cf.ReadLine(cf.PieceLinesCount-1).TrimEnd(','));
            }
            cf.WriteLine("}");
            cf.Save(false);
        }
        public override void ToFile(string path, string text)
        {
            File.WriteAllText(path, text, Encoding);
        }


        public string Pair(string key, IEnumerable<IEnumerable<KeyValuePair<string, string>>> kvpscollection) => string.Join(":", Normalize(key), Pair(kvpscollection));
        public string Pair(IEnumerable<IEnumerable<KeyValuePair<string, string>>> kvpscollection) => Collect(from v in kvpscollection select Pair(v));
        public string Pair(string key, IEnumerable<KeyValuePair<string, string>> kvps) => string.Join(":", Normalize(key), Pair(kvps));
        public string Pair(IEnumerable<KeyValuePair<string, string>> kvps) => Encapsulate(from v in kvps select Pair(v));
        public string Pair(KeyValuePair<string, string> kvp) => Pair(kvp.Key, kvp.Value);
        public string Pair(string key, IEnumerable<string> values) => string.Join(":", Normalize(key), Collect(values));
        public string Pair(string key, string value) => string.Join(":", Normalize(key), Normalize(value));
        public string Pair(string key, object value) => string.Join(":", Normalize(key), Normalize(value));
        public string Collect(IEnumerable<string> values) => "[" + string.Join(",", values) + "]";
        public string Encapsulate(IEnumerable<string> kvps) => "{" + string.Join(",", kvps) + "}";
        public string Encapsulate(params string[] kvps) => "{" + string.Join(",", kvps) + "}";
        public string Normalize(string value) => string.Join("", "\"", (value??"").Replace("\\", "\\\\").Replace("\"", "\\\""), "\"");
        public string Normalize(bool value) => value?"true":"false";
        public string Normalize(object value) => value + "";

    }
}
