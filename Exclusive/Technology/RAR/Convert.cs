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

namespace MiMFa.Exclusive.Technology.RAR
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
                    File.Copy(item, tempdir + Path.GetFileName(item), true);
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
        public virtual string ToDirectory(string path, string password = null)
        {
            return ToDirectory(path, TempDirectory + Path.GetFileName(path), password);
        }
        public virtual string ToDirectory(string path, string dir, string password = null)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //Put the path of installed winrar.exe
            proc.StartInfo.FileName = @"unrar.exe";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.EnableRaisingEvents = true;

            // Destination of your extraction directory
            string extractDir = dir.TrimEnd(Path.DirectorySeparatorChar);
            // Delete old extraction directory
            try
            {
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
                PathService.CreateAllDirectories(extractDir);
            }
            catch { }
            // Extract all of the files and dirs in your destination directory
            if (password==null) proc.StartInfo.Arguments = String.Format("x \"{0}\" \"{1}\"", path, extractDir);
            else proc.StartInfo.Arguments = String.Format("x -p{0} \"{1}\" \"{2}\"", password, path, extractDir);


            proc.Start();
            proc.WaitForExit();

            return dir;
        }

    }
}
