using iTextSharp.text.pdf;
using MiMFa.Model.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiMFa.Exclusive.Technology.Text
{
    public class Convert : Exclusive.Technology.Convert, IConvert
    {
        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }


        public override void ToFile(string path, string text)
        {
            File.WriteAllText(path,text, Encoding);
        }
        public override void ToFile(string path, IEnumerable<string> texts)
        {
            File.WriteAllText(path, string.Join(BreakSign ,texts), Encoding);
        }
        public override string ToText(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding, true);
            try
            {
                return sr.ReadToEnd();
            }
            finally { sr.Close(); }
        }
        public override IEnumerable<string> ToLines(string path)
        {
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs, Encoding, true);
            }
            catch { }
            try
            {
                while (sr.Peek() > 0)
                    yield return sr.ReadLine();
            }
            finally { sr.Close(); }
        }
        public override IEnumerable<IEnumerable<string>> ToRows(string path)
        {
            return ToChainedFile(path).ReadRows();
        }
    }
}
