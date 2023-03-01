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
using MiMFa.Network.Web;
using MiMFa.Model.IO;

namespace MiMFa.Exclusive.Technology.Web
{
    public class Convert : Exclusive.Technology.Convert, IConvert
    {
        public override string BreakSign { get; set; } = Environment.NewLine;


        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }


        public override string ToText(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding;
                return wc.DownloadString(url);
            }
        }
        public override IEnumerable<string> ToLines(string url)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding;
                return wc.DownloadString(url).Split(new string[] { BreakSign }, StringSplitOptions.None);
            }
        }
        public override IEnumerable<IEnumerable<string>> ToRows(string path)
        {
            return ToChainedFile(path).ReadRows();
        }
        public override ChainedFile ToChainedFile(string path)
        {
            return new ChainedFile(ToLines(path));
        }
        public override void ToFile(string sourUrl, string destFilePath)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding;
                wc.DownloadFile(sourUrl, destFilePath);
            }
        }
        public override void ToFile(string destDir, IEnumerable<string> sourUrls)
        {
            destDir = destDir.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar.ToString();
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding;
                foreach (var sourUrl in sourUrls)
                    wc.DownloadFile(sourUrl, destDir + ToFileName(sourUrl));
            }
        }
        public override string ToString(string url)
        {
            return File.ReadAllText(ToPath(url), Encoding);
        }

        public string ToPath(string url)
        {
            string path = TempDirectory + ToFileName(url);
            //if(DetailService.IsHTMLURL(url)) File.WriteAllText(path,ToText(url));
            //else 
            ToFile(url, path);
            return path;
        } 
        public string ToFileName(string url)
        {
            return  DateTime.Now.Ticks.ToString().Substring(10) + " " + ConvertService.ToAlphabeticName(url.Split('/').Last()) + ".htm";
        }
    }
}
