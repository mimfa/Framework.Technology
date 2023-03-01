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
using MiMFa.Service;
using MiMFa.Model.IO;

namespace MiMFa.Exclusive.Technology.Word
{
    public class Convert : Exclusive.Technology.Convert, IConvert
    {
        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }


        public override void ToFile(string path, string doc)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            // Destination of your extraction directory
            string extractDir = TempDirectory + Path.GetFileName(path) + separator;
            // Delete old extraction directory
            if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            try
            {   // Extract all of media an xml document in your destination directory
                if (File.Exists(path)) ZipFile.ExtractToDirectory(path, extractDir);
                else
                {
                    Directory.CreateDirectory(extractDir);
                    Directory.CreateDirectory(extractDir  + "word"+ separator);
                }

                File.WriteAllText(extractDir  +"word"+ separator+"document.xml", doc, Encoding);

                File.Delete(path);
                ZipFile.CreateFromDirectory(extractDir, path);
            }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }
        public override void ToFile(string path, IEnumerable<string> docs)
        {
            ToFile(path, string.Join(BreakSign, docs));
        }
        public override string ToText(string path)
        {
            return ToXmlDocument(path).DocumentElement.InnerText;
        }
        public override IEnumerable<string> ToLines(string path)
        {
            foreach (var item in ToRows(path))
                yield return string.Join(SplitSign,item);
        }
        public override IEnumerable<IEnumerable<string>> ToRows(string path)
        {
            // Read all text of your document from the XML
            foreach (XmlElement item in ToXmlDocument(path).DocumentElement.FirstChild.ChildNodes)
                if (item.HasChildNodes)
                {
                    List<string> ls = new List<string>();
                    foreach (XmlElement ch in item.ChildNodes)
                        ls.Add(ch.InnerText);
                    yield return ls;
                }
                else yield return new string[] { item.InnerText };
        }
        public override ChainedFile ToChainedFile(string path)
        {
            return new ChainedFile(ToRows(path));
        }
        public override string ToString(string path)
        {
            return ToXmlDocument(path).OuterXml;
        }
        public override IEnumerable<string> ToStrings(string path)
        {
            foreach (XmlNode item in ToXmlDocument(path))
                yield return item.OuterXml;
        }

        public virtual string ToDirectory(string path)
        {
            // Destination of your extraction directory
            string extractDir = TempDirectory  + Path.GetFileName(path) + Path.DirectorySeparatorChar.ToString();
            // Delete old extraction directory
            if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
                // Extract all of media an xml document in your destination directory
                ZipFile.ExtractToDirectory(path, extractDir);
            return extractDir;
        }

        public virtual string ToNote(string path)
        {
            return string.Join(BreakSign, ToNotes(path));
        }
        public virtual IEnumerable<string> ToNotes(string path)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            // Destination of your extraction directory
            string extractDir = ToDirectory(path);
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                // Read all text of your slides from the XML
                xmldoc.Load(extractDir + "word" + separator + "footnotes.xml");
                foreach (XmlElement item in xmldoc.DocumentElement.FirstChild.ChildNodes)
                    yield return item.InnerText;
            }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }

        public virtual string ToHeader(string path)
        {
            return string.Join(BreakSign, ToHeaders(path));
        }
        public virtual IEnumerable<string> ToHeaders(string path)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            // Destination of your extraction directory
            string extractDir = ToDirectory(path);
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                // Read all text of your slides from the XML
                foreach (string item in Directory.GetFiles(extractDir + "word" + separator, "header*"))
                {
                    xmldoc.Load(item);
                    yield return xmldoc.DocumentElement.InnerText;
                }
            }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }

        public virtual string ToFooter(string path)
        {
            return string.Join(BreakSign, ToFooters(path));
        }
        public virtual IEnumerable<string> ToFooters(string path)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            // Destination of your extraction directory
            string extractDir = ToDirectory(path);
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                // Read all text of your slides from the XML
                foreach (string item in Directory.GetFiles(extractDir + "word" + separator, "footer*"))
                {
                    xmldoc.Load(item);
                    yield return xmldoc.DocumentElement.InnerText;
                }
            }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }

        public virtual string ToContent(string path)
        {
            return string.Join(BreakSign, ToContents(path));
        }
        public virtual IEnumerable<string> ToContents(string path)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            // Destination of your extraction directory
            string extractDir = ToDirectory(path);
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                // Read all text of your slides from the XML
                xmldoc.Load(extractDir + "word" + separator + "document.xml");
                foreach (XmlElement item in xmldoc.DocumentElement.FirstChild.ChildNodes)
                    yield return item.InnerText;
                foreach (string item in Directory.GetFiles(extractDir + "word" + separator, "header*"))
                {
                    xmldoc.Load(item);
                    yield return xmldoc.DocumentElement.InnerText;
                }
                foreach (string item in Directory.GetFiles(extractDir + "word" + separator, "footer*"))
                {
                    xmldoc.Load(item);
                    yield return xmldoc.DocumentElement.InnerText;
                }
                xmldoc.Load(extractDir + "word" + separator + "footnotes.xml");
                foreach (XmlElement item in xmldoc.DocumentElement.FirstChild.ChildNodes)
                    yield return item.InnerText;
            }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }


        public virtual XmlDocument ToXmlDocument(string path)
        {
            string extractDir = ToDirectory(path);
            try { 
                XmlDocument xmldoc = new XmlDocument();
                // Load XML file contains all of your document text from the extracted XML file
                xmldoc.Load(extractDir +"word"+Path.DirectorySeparatorChar.ToString()+"document.xml");

                return xmldoc;
            }
            finally
            {
                // Delete extraction directory
                if ( Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }
        public virtual XElement ToXElement(string path)
        {
            string extractDir = ToDirectory(path);
            try
            {
                return XElement.Load(extractDir +"word"+ Path.DirectorySeparatorChar.ToString() + "document.xml");
            }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }

    }
}
