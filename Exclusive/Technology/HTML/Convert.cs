using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using MiMFa.Model;
using System.Windows.Forms;
using System.Data.OleDb;
using MiMFa.General;
using System.IO;
using System.IO.Compression;
using System.Xml;
using MiMFa.Network.Web;
using MiMFa.Service;
using MiMFa.Model.IO;

namespace MiMFa.Exclusive.Technology.HTML
{
    public class Convert : Technology.Convert
    {
        public virtual string Header { get; set; } = "";
        public virtual string Footer { get; set; } = "";
        public override string PageSign { get; set; } = "TABLE";
        public override string BreakSign { get; set; } = "TR";
        public override string SplitSign { get; set; } = "TD";
        public virtual string HeadSign { get; set; } = "TH";

        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }

        public void To(string srcAddress, string destAddress, Aspose.Words.SaveFormat format, bool forceOpen = true)
        {
            if (forceOpen)
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(srcAddress);
                doc.Save(destAddress, format);
                ProcessService.Run(destAddress);
            }
            else
            {
                System.Threading.Thread th = new System.Threading.Thread(() =>
                {
                    Aspose.Words.Document doc = new Aspose.Words.Document(srcAddress);
                    doc.Save(destAddress, format);
                });
                th.IsBackground = true;
                th.SetApartmentState(System.Threading.ApartmentState.STA);
                th.Start();
            }
        }


        public override ChainedFile ToChainedFile(string path)
        {
            return new ChainedFile(ToRows(path));
        }
        public override string ToText(string path)
        {
            HtmlDocument elem = ToHtmlDocument(path);
            if (elem != null && elem.Body == null) return null;
            if (string.IsNullOrWhiteSpace(PageSign))
                return elem.Body.InnerText;
            else
            {
                List<string> lst = new List<string>();
                foreach (HtmlElement item in elem.GetElementsByTagName(PageSign))
                    lst.Add(item.InnerText);
                return string.Join("", lst);
            }
        }
        public override IEnumerable<string> ToLines(string path)
        {
            HtmlDocument elem = ToHtmlDocument(path);
            if (string.IsNullOrWhiteSpace(PageSign))
                return ToLines(elem.Body);
            else return ToLines(elem.Body, PageSign);
        }
        public override IEnumerable<IEnumerable<string>> ToRows(string path)
        {
            HtmlDocument elem = ToHtmlDocument(path);
            if (string.IsNullOrWhiteSpace(PageSign))
                return ToRows(elem.Body);
            else return ToRows(elem.Body, PageSign);
        }
        public virtual IEnumerable<string> ToLines(HtmlElement elem, string pageSing)
        {
            foreach (HtmlElement item in elem.GetElementsByTagName(pageSing))
                foreach (var e in ToLines(item))
                    yield return e;
        }
        public virtual IEnumerable<IEnumerable<string>> ToRows(HtmlElement elem, string pageSing)
        {
            foreach (HtmlElement item in elem.GetElementsByTagName(pageSing))
                foreach (var e in ToRows(item))
                    yield return e;
        }
        public virtual IEnumerable<string> ToLines(HtmlElement elem)
        {
            if (elem == null) yield break;
            if (string.IsNullOrWhiteSpace(BreakSign))
                if (elem.Children.Count > 0)
                    foreach (HtmlElement ch in elem.Children)
                        yield return ch.InnerText;
                else yield return elem.InnerText;
            else
                if (elem.Children.Count > 0)
                foreach (HtmlElement ch in elem.GetElementsByTagName(BreakSign))
                    yield return ch.InnerText;
            else yield break;
        }
        public virtual IEnumerable<IEnumerable<string>> ToRows(HtmlElement elem)
        {
            if (elem == null) yield break;
            if (string.IsNullOrWhiteSpace(BreakSign))
                if (elem.Children.Count > 0)
                    foreach (HtmlElement ch in elem.Children)
                        yield return ToCells(ch);
                else yield return ToCells(elem);
            else
                if (elem.Children.Count > 0)
                foreach (HtmlElement ch in elem.GetElementsByTagName(BreakSign))
                    yield return ToCells(ch);
            else yield break;
        }
        public virtual IEnumerable<string> ToCells(HtmlElement elem)
        {
            if (string.IsNullOrWhiteSpace(SplitSign))
                if (elem.Children.Count > 0)
                    foreach (HtmlElement ch in elem.Children)
                        yield return ch.InnerText;
                else yield return elem.InnerText;
            else if (elem.Children.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(HeadSign))
                    foreach (HtmlElement ch in elem.GetElementsByTagName(HeadSign))
                        yield return ch.InnerText;
                foreach (HtmlElement ch in elem.GetElementsByTagName(SplitSign))
                    yield return ch.InnerText;
            }
            else yield break;
        }
        
        public virtual string ToString(HtmlDocument htmldoc)
        {
            if (htmldoc!= null && htmldoc.Body == null) return null;
            return htmldoc.Body.InnerHtml;
        }
        public virtual IEnumerable<string> ToStrings(HtmlDocument htmldoc)
        {
            if (htmldoc!= null && htmldoc.Body == null) yield break;
            foreach (HtmlElement item in htmldoc.Body.Children)
                yield return item.InnerHtml;
        }

        public virtual void ToFile(string htmlPath, HtmlDocument htmldocs)
        {
            ToFile(htmlPath,ToString(htmldocs));
        }

        IEnumerable<string> Labels = null;
        public override void ToFile(string path, ChainedFile doc)
        {
            if (doc.HasPieceColumnsLabels)
            {
                Labels = from v in doc.PieceColumnsLabels select StringService.ToHTML(v);
                ToFile(path, doc.ReadRows(doc.PieceColumnsLabelsIndex+1));
            }
            else ToFile(path, doc.ReadRows());
        }
        public override void ToFile(string path, IEnumerable<IEnumerable<string>> cells)
        {
            string ss = string.IsNullOrWhiteSpace(SplitSign) ? "TD" : SplitSign;
            string j = string.Join("", "</", ss, ">", "<", ss, ">");
            ToFile(path, from row in cells select string.Join(j, from v in row select StringService.ToHTML(v)));
        }
        public override void ToFile(string path, IEnumerable<string> lines)
        {
            ChainedFile cf = new ChainedFile(path,Encoding);
            cf.Clear();
            string ps = string.IsNullOrWhiteSpace(PageSign) ? "TABLE" : PageSign;
            string hs = string.IsNullOrWhiteSpace(HeadSign) ? "TH" : HeadSign;
            string bs = string.IsNullOrWhiteSpace(BreakSign) ? "TR" : BreakSign;
            string ss = string.IsNullOrWhiteSpace(SplitSign) ? "TD" : SplitSign;
            cf.WriteLine(Header+"<" + ps +">");
            if(Labels !=null) cf.WriteLine(string.Join("", "<"+bs+"><"+ hs + ">", string.Join("</" + hs + "><" + hs + ">", Labels), "</"+ hs + "></"+bs+">"));
            string sj = string.Join("", "<", bs , ">", "<", ss, ">");
            string ej = string.Join("", "</", ss, ">", "</", bs, ">");
            cf.WriteLines(from v in lines select string.Join("", sj, v, ej));
            cf.WriteLine("</"+ps+">"+ Footer);
            cf.Save(false);
        }

        public virtual HtmlDocument ToHtmlDocument(string htmlPath)
        {
            return ToDocument(File.ReadAllText(htmlPath, Encoding));
        }
        public new HtmlDocument ToDocument(string html)
        {
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.DocumentText = html;
            browser.Document.OpenNew(true);
            browser.Document.Write(html);
            browser.Refresh();
            return browser.Document;
        }
    }
}
