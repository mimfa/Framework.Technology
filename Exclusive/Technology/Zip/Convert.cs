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
using System.Xml.Linq;
using MiMFa.Model.IO;

namespace MiMFa.Exclusive.Technology.Zip
{
    public class Convert : Exclusive.Technology.Convert, IConvert
    {
        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }


        public override void ToFile(string path, string extractedDir)
        {
            if (!Directory.Exists(extractedDir)) PathService.CreateAllDirectories(extractedDir);
            if (File.Exists(path)) File.Delete(path);

            ZipFile.CreateFromDirectory(extractedDir, path);
        }
        public override void ToFile(string path, IEnumerable<string> paths)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            ToFile(path, paths, TempDirectory + Path.GetFileName(path) + separator);
        }
        public void ToFile(string path, IEnumerable<string> paths, string tempdir)
        {
            // Delete old extraction directory
            if (Directory.Exists(tempdir)) try { Directory.Delete(tempdir, true); } catch { }
            else Directory.CreateDirectory(tempdir);
            try
            {
                foreach (var item in paths)
                    if(Directory.Exists(item)) PathService.CopyDirectory(item, tempdir + PathService.GetDirectoryName(item), true);
                    else File.Copy(item, tempdir + Path.GetFileName(item), true);
                ToFile(path, tempdir);
            }
            catch { }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(tempdir)) Directory.Delete(tempdir, true);
            }
        }
        public virtual IEnumerable<string> ToFiles(string path)
        {
            string addr = ToDirectory(path);
            return Directory.GetFiles(addr, "*", SearchOption.AllDirectories);
        }
        public override string ToText(string path)
        {
            return string.Join(Environment.NewLine, ToLines(path));
        }
        public override IEnumerable<string> ToLines(string path)
        {
            return from addr in ToFiles(path) select path.Replace(path, "");
        }
        public override IEnumerable<IEnumerable<string>> ToRows(string path)
        {
            return ToChainedFile(path).ReadRows();
        }
        public override ChainedFile ToChainedFile(string path)
        {
            return new ChainedFile(ToLines(path));
        }
        public override string ToString(string path)
        {
            return string.Join(BreakSign, ToStrings(path));
        }
        public override IEnumerable<string> ToStrings(string path)
        {
            return from addr in ToFiles(path) select File.ReadAllText(path);
        }
        public virtual string ToDirectory(string path)
        {
            return ToDirectory(path, TempDirectory + Path.GetFileName(path),true);
        }
        public virtual string ToDirectory(string path, string dir, bool cleardir = true)
        {
            // Destination of your extraction directory
            string extractDir = dir.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar.ToString();
            // Delete old extraction directory
            if (cleardir)
                try
                {
                    if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
                }
                catch { }
            // Extract all of media an xml document in your destination directory
            ZipFile.ExtractToDirectory(path, extractDir);
            return extractDir;
        }

    }
}
