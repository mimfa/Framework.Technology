using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using MiMFa.Model;
using System.Windows.Forms;
using System.Data.OleDb;
using MiMFa.General;
using System.Drawing;

namespace MiMFa.Exclusive.Technology
{
    public interface IConvert
    {
        System.Text.Encoding Encoding { get; set; }


        string TempDirectory { get; }
        string BreakSign { get; set; }

        void ClearTemp();

        string ToString(string path);
        IEnumerable<string> ToStrings(string path);

        string ToText(string path);
        IEnumerable<string> ToLines(string path);

        void ToFile(string path, string text);
        void ToFile(string path, IEnumerable<string> texts);
    }
}
