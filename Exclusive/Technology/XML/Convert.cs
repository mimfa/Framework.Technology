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
using System.Xml.Linq;
using MiMFa.Model.IO;
using MiMFa.Service;

namespace MiMFa.Exclusive.Technology.XML
{
    public class Convert : Exclusive.Technology.Convert
    {
        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }


        public virtual string Header { get; set; } = "";
        public virtual string Footer { get; set; } = "";
        public override string PageSign { get; set; } = null;
        public override string BreakSign { get; set; } = null;
        public override string SplitSign { get; set; } = null;

        public override string ToText(string path)
        {
            if (string.IsNullOrWhiteSpace(PageSign))
                return ToXmlDocument(path).InnerText;
            else
            {
                var elem = ToXmlDocument(path);
                List<string> lst = new List<string>();
                foreach (XmlElement item in elem.GetElementsByTagName(PageSign))
                        lst.Add(item.InnerText);
                return string.Join("", lst);
            }
        }
        public override IEnumerable<string> ToLines(string path)
        {
            var elem = ToXmlDocument(path);
            if (string.IsNullOrWhiteSpace(PageSign))
                return ToLines(elem.DocumentElement);
            else return ToLines(elem.DocumentElement, PageSign);
        }
        public override IEnumerable<IEnumerable<string>> ToRows(string path)
        {
            var elem = ToXmlDocument(path);
            if (string.IsNullOrWhiteSpace(PageSign))
                return ToRows(elem.DocumentElement);
            else return ToRows(elem.DocumentElement, PageSign);
        }
        public virtual IEnumerable<string> ToLines(XmlElement elem, string pageSing)
        {
            foreach (XmlElement item in elem.GetElementsByTagName(pageSing))
                foreach (var e in ToLines(item))
                    yield return e;
        }
        public virtual IEnumerable<IEnumerable<string>> ToRows(XmlElement elem, string pageSing)
        {
            foreach (XmlElement item in elem.GetElementsByTagName(pageSing))
                foreach (var e in ToRows(item))
                    yield return e;
        }
        public virtual IEnumerable<string> ToLines(XmlElement elem)
        {
            if (string.IsNullOrWhiteSpace(BreakSign))
                if (elem.HasChildNodes)
                    foreach (XmlElement ch in elem.ChildNodes)
                        yield return ch.InnerText;
                else yield return elem.InnerText;
            else
                if (elem.HasChildNodes)
                    foreach (XmlElement ch in elem.GetElementsByTagName(BreakSign))
                        yield return ch.InnerText;
                else yield break;
        }
        public virtual IEnumerable<IEnumerable<string>> ToRows(XmlElement elem)
        {
            if (string.IsNullOrWhiteSpace(BreakSign))
                if (elem.HasChildNodes)
                    foreach (XmlElement ch in elem.ChildNodes)
                        yield return ToCells(ch);
                else yield return ToCells(elem);
            else
                if (elem.HasChildNodes)
                foreach (XmlElement ch in elem.GetElementsByTagName(BreakSign))
                    yield return ToCells(ch);
            else yield break;
        }
        public virtual IEnumerable<string> ToCells(XmlElement elem)
        {
            if (string.IsNullOrWhiteSpace(SplitSign))
                if (elem.HasChildNodes)
                    foreach (XmlElement ch in elem.ChildNodes)
                        yield return ch.InnerText;
                else yield return elem.InnerText;
            else
                if (elem.HasChildNodes)
                foreach (XmlElement ch in elem.GetElementsByTagName(SplitSign))
                    yield return ch.InnerText;
            else yield break;
        }
        public virtual string ToText(XmlDocument xmldoc)
        {
            return xmldoc.DocumentElement.InnerText;
        }
        public virtual IEnumerable<string> ToTexts(XmlDocument xmldoc)
        {
            foreach (XmlElement item in xmldoc.ChildNodes)
               yield return item.InnerText;
        }

        private List<string> Labels;
        public override void ToFile(string path, ChainedFile doc)
        {
            Labels = new List<string>();
            for (int i = 0; i < doc.PieceWarpsCount; i++)
                Labels.Add(ConvertService.ToAlphabet(i));
            ToFile(path, doc.ReadRows());
        }
        public virtual void ToFile(string xmlPath, XmlDocument xmldoc)
        {
            xmldoc.Save(xmlPath);
        }
        public override void ToFile(string path, IEnumerable<IEnumerable<string>> cells)
        {
            if (string.IsNullOrWhiteSpace(SplitSign))
            {
                int i = 0;
                ToFile(path,
                    from row in cells
                    where (i = 0) > -1
                    select string.Join("",
                        from c in row
                        let lab = Labels.Count > i ? Labels[i++] : ConvertService.ToAlphabet(i++)
                        select string.Join("", "<", lab, ">", StringService.ToXML(c), "</", lab, ">")));
            }
            else
            {
                int i = 0;
                string sj = "<" + SplitSign + ">";
                string ej = "</" + SplitSign + ">";
                ToFile(path,
                    from row in cells
                    where (i = 0) > -1
                    select string.Join("",
                        from c in row
                        select string.Join("", sj, StringService.ToXML(c), ej)));
            }
        }
        public override void ToFile(string path, IEnumerable<string> lines)
        {
            ChainedFile cf = new ChainedFile(path, Encoding);
            cf.Clear();
            cf.WriteLine(Header);
            string ps = string.IsNullOrWhiteSpace(PageSign) ? "DOCUMENT" : PageSign;
            string bs = string.IsNullOrWhiteSpace(PageSign) ? "LINE" : BreakSign;
            cf.WriteLine("<"+ps+">");
            string sj = "<" + bs + ">";
            string ej = "</" + bs + ">";
            cf.WriteLines(from v in lines select string.Join("", sj, v, ej));
            cf.WriteLine("</"+ ps + ">");
            cf.WriteLine(Footer);
            cf.Save(false);
        }

        public virtual XmlDocument ToXmlDocument(string xmlpath)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlpath);
            return xmldoc;
        }
        public virtual XmlDocument ToDocument(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            return xmldoc;
        }

        public virtual XElement ToXElement(string xmlpath)
        {
            return XElement.Load(xmlpath);
        }
        public virtual XElement ToElement(string xml)
        {
            return XElement.Parse(xml);
        }

    }
}
