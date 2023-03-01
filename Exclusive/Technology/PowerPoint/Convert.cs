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

namespace MiMFa.Exclusive.Technology.PowerPoint
{
    public class Convert : Exclusive.Technology.Convert, IConvert
    {
        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }


        public override void ToFile(string path, string doc)
        {
            ToFile(path, doc.Split(new string[] { BreakSign},StringSplitOptions.None));
        }
        public override void ToFile(string path, IEnumerable<string> docs)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            // Destination of your extraction directory
            string extractDir = TempDirectory + Path.GetFileName(path)+separator;
            string addr = extractDir + "ppt"+separator +"slides"+separator ;
            // Delete old extraction directory
            if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            try
            {   // Extract all of media an xml document in your destination directory
                if (File.Exists(path)) ZipFile.ExtractToDirectory(path, extractDir);
                else
                {
                    Directory.CreateDirectory(extractDir);
                    Directory.CreateDirectory(extractDir+ "ppt"+separator );
                    Directory.CreateDirectory(addr);
                }

                int i = 1;
                foreach (var item in docs)
                    File.WriteAllText(addr + "slide" + (i++)+".xml", item, Encoding);

                File.Delete(path);
                ZipFile.CreateFromDirectory(extractDir, path);
            }
            catch { }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }
        public override string ToText(string path)
        {
            return string.Join(BreakSign, ToLines(path));
        }
        public override IEnumerable<string> ToLines(string path)
        {
            // Read all text of your document from the XML
            foreach (var doc in ToXmlDocuments(path))
                foreach (XmlNode item in doc.DocumentElement.FirstChild.ChildNodes)
                    foreach (XmlNode ch in GetLastChildren(item))
                        yield return ch.InnerText;
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
            foreach (XmlDocument item in ToXmlDocuments(path))
                yield return item.OuterXml;
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
                foreach (string item in Directory.GetFiles(extractDir + "ppt"+separator +"notesSlides"+separator , "*.xml"))
                {
                    // Load XML file contains all of your slide text from the extracted XML file
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
                foreach (string item in Directory.GetFiles(extractDir + "ppt" + separator + "slides" + separator, "*.xml"))
                {
                    // Load XML file contains all of your slide text from the extracted XML file
                    xmldoc.Load(item);
                    yield return xmldoc.InnerText;
                }
                // Read all text of your slides from the XML
                foreach (string item in Directory.GetFiles(extractDir + "ppt" + separator + "notesSlides" + separator, "*.xml"))
                {
                    // Load XML file contains all of your slide text from the extracted XML file
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


        public virtual string ToDirectory(string path)
        {
            // Destination of your extraction directory
            string extractDir = TempDirectory + Path.GetFileName(path) + Path.DirectorySeparatorChar.ToString();
            // Delete old extraction directory
            if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
                // Extract all of media an xml document in your destination directory
                ZipFile.ExtractToDirectory(path, extractDir);
            return extractDir;
        }

        public virtual IEnumerable<XmlDocument> ToXmlDocuments(string path)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            // Destination of your extraction directory
            string extractDir = ToDirectory(path);
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                // Read all text of your slides from the XML
                foreach (string item in Directory.GetFiles(extractDir + "ppt"+separator +"slides"+separator , "*.xml"))
                {
                    // Load XML file contains all of your slide text from the extracted XML file
                    xmldoc.Load(item);
                    yield return xmldoc;
                }
            }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }
        public virtual IEnumerable<XElement> ToXElements(string path)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            string extractDir = ToDirectory(path);
            try
            {
                foreach (string item in Directory.GetFiles(extractDir+ "ppt"+separator +"slides"+separator , "*.xml"))
                {
                    // Load XML file contains all of your slide text from the extracted XML file
                    yield return XElement.Load(item);
                }
            }
            finally
            {
                // Delete extraction directory
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
            }
        }

        public virtual IEnumerable<XmlNode> GetLastChildren(XmlNode xml)
        {
            if (xml.HasChildNodes)
                foreach (XmlNode item in xml.ChildNodes)
                    foreach (XmlNode ch in GetLastChildren(item))
                        yield return ch;
            else yield return xml;
        }
    }
}
