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
using System.Runtime.InteropServices;
using System.Drawing;
using Svg;

namespace MiMFa.Exclusive.Technology.SVG
{
    public class Convert : Exclusive.Technology.Convert, IConvert
    {
        public virtual Image ToImage(string svgPath, int width = -1, int height = -1)
        {
            Svg.SvgDocument doc = Svg.SvgDocument.Open(svgPath);
            doc.ShapeRendering = SvgShapeRendering.Auto;
            return doc.Draw(width, height);
        }
        public virtual Image ToImage(string svgPath, Size size)
        {
            return ToImage(svgPath,size.Width, size.Height);
        }
        public virtual void ToFile(string svgPath, string imagePath, int width, int height = -1, System.Drawing.Imaging.ImageFormat type = null)
        {
            var img = ToImage(svgPath, width, height);
            img.Save(imagePath, type??System.Drawing.Imaging.ImageFormat.Png);
        }
        public override void ToFile(string svgPath, string imagePath)
        {
            ToFile(svgPath, imagePath,-1,-1);
        }
        public override void ToFile(string path, IEnumerable<string> paths)
        {
            foreach (var item in paths)
                ToFile(path, item, -1, -1);
        }
    }
}
