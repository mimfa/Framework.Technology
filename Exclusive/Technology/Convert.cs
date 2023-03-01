using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using MiMFa.Model;
using System.Windows.Forms;
using System.Data.OleDb;
using MiMFa.Service;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Text;
using MiMFa.Model.IO;

namespace MiMFa.Exclusive.Technology
{
    public class Convert : IConvert
    {
        public virtual Encoding Encoding { get; set; } = Encoding.UTF8;

        public virtual string TempDirectory
        {
            get
            {
                string dir = Config.TemporaryDirectory + "Convert" + Path.DirectorySeparatorChar.ToString() + GetType().Namespace.Split('.').Last() + Path.DirectorySeparatorChar.ToString();
                PathService.CreateAllDirectories(dir);
                return dir;
            }
        }
        public virtual string PageSign { get; set; } = "\r\n\r\n";
        public virtual string BreakSign { get; set; } = "\r\n";
        public virtual string SplitSign { get; set; } = "\t";

        public Convert(bool clearTemporary = true)
        {
            Licenses.ActivateMemoryPatching();
            if (!Statement.IsDesignTime && clearTemporary) ClearTemp();
        }


        public virtual void ToFile(string path, ChainedFile doc)
        {
            ToFile(path,doc.ReadRows());
        }
        public virtual void ToFile(string path, IEnumerable<IEnumerable<string>> cells)
        {
            ToFile(path, from v in cells select string.Join(SplitSign, v));
        }
        public virtual void ToFile(string path, IEnumerable<string> texts)
        {
            File.WriteAllText(path,string.Join(BreakSign, texts),Encoding);
        }
        public virtual void ToFile(string path, string text)
        {
            File.WriteAllText(path, text, Encoding);
        }


        public virtual void ClearTemp()
        {
            if (Directory.Exists(TempDirectory)) Directory.Delete(TempDirectory,true);
        }


        public virtual ChainedFile ToChainedFile(string path)
        {
            var doc = new ChainedFile(path);
            doc.Update();
            return doc;
        }
        public virtual string ToText(string path)
        {
            return ToString(path);
        }
        public virtual IEnumerable<string> ToLines(string path)
        {
            return ToChainedFile(path).ReadLines();
        }
        public virtual IEnumerable<IEnumerable<string>> ToRows(string path)
        {
            return ToChainedFile(path).ReadRows();
        }

        public virtual string ToString(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding,  true);
            try
            {
                return sr.ReadToEnd();
            }
            finally { sr.Close(); }
        }
        public virtual IEnumerable<string> ToStrings(string path)
        {
            return ToLines(path);
        }
    }
}
