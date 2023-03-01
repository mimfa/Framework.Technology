using iTextSharp.text.pdf;
using MiMFa.Model.IO;
using MiMFa.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiMFa.Exclusive.Technology.PDF
{
    public class Convert : Exclusive.Technology.HTML.Convert
    {
        public override string PageSign { get; set; } = Environment.NewLine+ Environment.NewLine;
        public override string BreakSign { get; set; } = Environment.NewLine;
        public override string SplitSign { get; set; } = "\t";

        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }


        public override ChainedFile ToChainedFile(string path)
        {
            return new ChainedFile(ToRows(path));
        }
        public override string ToText(string path)
        {
            return string.Join(PageSign, ToPages(path));
        }
        public virtual IEnumerable<string> ToPages(string path)
        {
            PdfReader reader = new PdfReader(path);
            int numPage = reader.NumberOfPages;
            for (int page = 1; page <= numPage; page++)
                yield return ToText(reader.GetPageContent(page));
        }
        public override IEnumerable<string> ToLines(string path)
        {
            string[] sp = { BreakSign };
            foreach (var page in ToPages(path))
                foreach (var item in page.Split(sp, StringSplitOptions.None))
                    yield return item;
        }
        public override IEnumerable<IEnumerable<string>> ToRows(string path)
        {
            string[] sp = { SplitSign };
            foreach (var item in ToLines(path))
                yield return item.Split(sp,StringSplitOptions.None);
        }

        public string ToText(string pdfAddress, int beginPageNumber = 1, int maxNumberOfPage = -1)
        {
            int numPage = maxNumberOfPage;
            List<string> result = new List<string>();
            PdfReader reader = new PdfReader(pdfAddress);
            numPage = maxNumberOfPage < 0 ? reader.NumberOfPages : Math.Min(maxNumberOfPage, reader.NumberOfPages);
            int error = 1;
            for (int page = beginPageNumber; page <= numPage; page++)
                try { result.Add(ToText(reader.GetPageContent(page))); }
                catch { if (error++ > 5) break; }
            string pdf = string.Join(PageSign, result);
            if (string.IsNullOrEmpty(pdf))
                throw new Exception();
            return pdf;
        }
        public string ToText(byte[] input)
        {
            if (input == null || input.Length == 0) return "";
            try
            {
                List<string> results = new List<string>();
                bool isEscapedChar = false;
                bool isTextObject = false;
                bool isNormalText = false;
                List<char> previousCharacters = new List<char>() { ' ', ' ', ' ', ' ', ' '};
                for (int i = 0; i < input.Length; i++)
                {
                    char c = (char)input[i];
                    //results.Add(c.ToString());
                    //continue;
                    previousCharacters.Add(char.Parse(c.ToString().ToLower()));
                    previousCharacters.RemoveAt(0);

                    if (!isTextObject) isTextObject = CheckToken(previousCharacters, "bt"); // Start of a text object
                    else if (isNormalText)// Just a normal text character:
                    {
                        // Only print out next character no matter what. 
                        if (c == ')' && !isEscapedChar)
                            isNormalText = false; // Stop outputting text
                        else if (c == '\\' && !isEscapedChar)
                            isEscapedChar = true; // Do not interpret.
                        else if (!InfoService.IsControlCharNumber(c))
                        {
                            results.Add(c.ToString());
                            isEscapedChar = false;
                        }
                    }
                    else // Position the text
                    {
                        if (CheckToken(previousCharacters, "td"))
                            results.Add("\n");
                        else if (CheckToken(previousCharacters, "t*"))// "'", "\"", 
                            results.Add("\r\n");
                        else if (CheckToken(previousCharacters, "tj"))
                            results.Add(" ");
                        else if (CheckToken(previousCharacters, "et"))
                        {
                            isTextObject = false;
                            results.Add("\t");
                        }// End of a text object, also going to a new line.
                        else if (c == '(' && !isEscapedChar)
                            isNormalText = true; // Start outputting text
                    }
                }
                return Service.StringService.ReplaceWordsBetween(string.Join("", results).Trim(), "\n", "\n",
                    (sm) =>
                    {
                        if (string.IsNullOrWhiteSpace(sm)) return "§{newbreakline}§";
                        return sm;
                    }, true).Replace(" \n ", "§{ newline}§")
                            .Replace(" \n", "§{ newline}§")
                            .Replace("\n ", "§{ newline}§")
                            .Replace(" \t  \t", "§{ tab}§")
                            .Replace("§{newbreakline}§", "\r\n")
                            .Replace("§{ newline}§", "\n")
                            .Replace("§{ tab}§", "\t")
                            .Replace("  ", " ");
            }
            catch
            {
                return "";
            }
        }
        internal bool CheckToken(List<char> recent, params string[] tokens)
        {
            foreach (string token in tokens)
                if (
                        (
                            recent[recent.Count - 3] == token[0] &&
                            token.Length > 1 &&
                            recent[recent.Count - 2] == token[1]
                        ) &&
                        (
                            recent[recent.Count - 1] == ' ' ||
                            recent[recent.Count - 1] == ')' ||
                            recent[recent.Count - 1] == '\r' ||
                            recent[recent.Count - 1] == '\n'
                        ) &&
                        (
                            recent[recent.Count - 4] == ' ' ||
                            recent[recent.Count - 4] == ')' ||
                            recent[recent.Count - 4] == '\r' ||
                            recent[recent.Count - 4] == '\n'
                        )
                    )
                    return true;
            return false;
        }

        public bool ToTextFile(string pdfAddress, string textAddress, int beginPageNumber = 1, int maxNumberOfPage = -1)
        {
            StreamWriter outFile = null;
            try
            {
                string text = ToText(pdfAddress, beginPageNumber, maxNumberOfPage);
                if (text != null)
                {
                    outFile = new StreamWriter(textAddress, false, Encoding);
                    outFile.Write(text);
                }
                else return false;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (outFile != null) outFile.Close();
            }
        }
    }
}
