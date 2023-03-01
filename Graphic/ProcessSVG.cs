using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.IO;
using Svg;
using System.Xml;
using System.Text.RegularExpressions;
using MiMFa.Service;

namespace MiMFa.Graphic
{
    public class ProcessSVG : ProcessGraphic<SvgDocument>
    {
        public virtual IEnumerable<KeyValuePair<string,string>> ColorsCodes
        {
            get
            {
                {
                    yield return new KeyValuePair<string, string>("aliceblue", "#f0f8ff");
                    yield return new KeyValuePair<string, string>("antiquewhite", "#faebd7");
                    yield return new KeyValuePair<string, string>("aqua", "#00ffff");
                    yield return new KeyValuePair<string, string>("aquamarine", "#7fffd4");
                    yield return new KeyValuePair<string, string>("azure", "#f0ffff");
                    yield return new KeyValuePair<string, string>("beige", "#f5f5dc");
                    yield return new KeyValuePair<string, string>("bisque", "#ffe4c4");
                    yield return new KeyValuePair<string, string>("black", "#000000");
                    yield return new KeyValuePair<string, string>("blanchedalmond", "#ffebcd");
                    yield return new KeyValuePair<string, string>("blue", "#0000ff");
                    yield return new KeyValuePair<string, string>("blueviolet", "#8a2be2");
                    yield return new KeyValuePair<string, string>("brown", "#a52a2a");
                    yield return new KeyValuePair<string, string>("burlywood", "#deb887");
                    yield return new KeyValuePair<string, string>("cadetblue", "#5f9ea0");
                    yield return new KeyValuePair<string, string>("chartreuse", "#7fff00");
                    yield return new KeyValuePair<string, string>("chocolate", "#d2691e");
                    yield return new KeyValuePair<string, string>("coral", "#ff7f50");
                    yield return new KeyValuePair<string, string>("cornflowerblue", "#6495ed");
                    yield return new KeyValuePair<string, string>("cornsilk", "#fff8dc");
                    yield return new KeyValuePair<string, string>("crimson", "#dc143c");
                    yield return new KeyValuePair<string, string>("cyan", "#00ffff");
                    yield return new KeyValuePair<string, string>("darkblue", "#00008b");
                    yield return new KeyValuePair<string, string>("darkcyan", "#008b8b");
                    yield return new KeyValuePair<string, string>("darkgoldenrod", "#b8860b");
                    yield return new KeyValuePair<string, string>("darkgray", "#a9a9a9");
                    yield return new KeyValuePair<string, string>("darkgreen", "#006400");
                    yield return new KeyValuePair<string, string>("darkgrey", "#a9a9a9");
                    yield return new KeyValuePair<string, string>("darkkhaki", "#bdb76b");
                    yield return new KeyValuePair<string, string>("darkmagenta", "#8b008b");
                    yield return new KeyValuePair<string, string>("darkolivegreen", "#556b2f");
                    yield return new KeyValuePair<string, string>("darkorange", "#ff8c00");
                    yield return new KeyValuePair<string, string>("darkorchid", "#9932cc");
                    yield return new KeyValuePair<string, string>("darkred", "#8b0000");
                    yield return new KeyValuePair<string, string>("darksalmon", "#e9967a");
                    yield return new KeyValuePair<string, string>("darkseagreen", "#8fbc8f");
                    yield return new KeyValuePair<string, string>("darkslateblue", "#483d8b");
                    yield return new KeyValuePair<string, string>("darkslategray", "#2f4f4f");
                    yield return new KeyValuePair<string, string>("darkslategrey", "#2f4f4f");
                    yield return new KeyValuePair<string, string>("darkturquoise", "#00ced1");
                    yield return new KeyValuePair<string, string>("darkviolet", "#9400d3");
                    yield return new KeyValuePair<string, string>("deeppink", "#ff1493");
                    yield return new KeyValuePair<string, string>("deepskyblue", "#00bfff");
                    yield return new KeyValuePair<string, string>("dimgray", "#696969");
                    yield return new KeyValuePair<string, string>("dimgrey", "#696969");
                    yield return new KeyValuePair<string, string>("dodgerblue", "#1e90ff");
                    yield return new KeyValuePair<string, string>("firebrick", "#b22222");
                    yield return new KeyValuePair<string, string>("floralwhite", "#fffaf0");
                    yield return new KeyValuePair<string, string>("forestgreen", "#228b22");
                    yield return new KeyValuePair<string, string>("fuchsia", "#ff00ff");
                    yield return new KeyValuePair<string, string>("gainsboro", "#dcdcdc");
                    yield return new KeyValuePair<string, string>("ghostwhite", "#f8f8ff");
                    yield return new KeyValuePair<string, string>("goldenrod", "#daa520");
                    yield return new KeyValuePair<string, string>("gold", "#ffd700");
                    yield return new KeyValuePair<string, string>("gray", "#808080");
                    yield return new KeyValuePair<string, string>("green", "#008000");
                    yield return new KeyValuePair<string, string>("greenyellow", "#adff2f");
                    yield return new KeyValuePair<string, string>("grey", "#808080");
                    yield return new KeyValuePair<string, string>("honeydew", "#f0fff0");
                    yield return new KeyValuePair<string, string>("hotpink", "#ff69b4");
                    yield return new KeyValuePair<string, string>("indianred", "#cd5c5c");
                    yield return new KeyValuePair<string, string>("indigo", "#4b0082");
                    yield return new KeyValuePair<string, string>("ivory", "#fffff0");
                    yield return new KeyValuePair<string, string>("khaki", "#f0e68c");
                    yield return new KeyValuePair<string, string>("lavenderblush", "#fff0f5");
                    yield return new KeyValuePair<string, string>("lavender", "#e6e6fa");
                    yield return new KeyValuePair<string, string>("lawngreen", "#7cfc00");
                    yield return new KeyValuePair<string, string>("lemonchiffon", "#fffacd");
                    yield return new KeyValuePair<string, string>("lightblue", "#add8e6");
                    yield return new KeyValuePair<string, string>("lightcoral", "#f08080");
                    yield return new KeyValuePair<string, string>("lightcyan", "#e0ffff");
                    yield return new KeyValuePair<string, string>("lightgoldenrodyellow", "#fafad2");
                    yield return new KeyValuePair<string, string>("lightgray", "#d3d3d3");
                    yield return new KeyValuePair<string, string>("lightgreen", "#90ee90");
                    yield return new KeyValuePair<string, string>("lightgrey", "#d3d3d3");
                    yield return new KeyValuePair<string, string>("lightpink", "#ffb6c1");
                    yield return new KeyValuePair<string, string>("lightsalmon", "#ffa07a");
                    yield return new KeyValuePair<string, string>("lightseagreen", "#20b2aa");
                    yield return new KeyValuePair<string, string>("lightskyblue", "#87cefa");
                    yield return new KeyValuePair<string, string>("lightslategray", "#778899");
                    yield return new KeyValuePair<string, string>("lightslategrey", "#778899");
                    yield return new KeyValuePair<string, string>("lightsteelblue", "#b0c4de");
                    yield return new KeyValuePair<string, string>("lightyellow", "#ffffe0");
                    yield return new KeyValuePair<string, string>("lime", "#00ff00");
                    yield return new KeyValuePair<string, string>("limegreen", "#32cd32");
                    yield return new KeyValuePair<string, string>("linen", "#faf0e6");
                    yield return new KeyValuePair<string, string>("magenta", "#ff00ff");
                    yield return new KeyValuePair<string, string>("maroon", "#800000");
                    yield return new KeyValuePair<string, string>("mediumaquamarine", "#66cdaa");
                    yield return new KeyValuePair<string, string>("mediumblue", "#0000cd");
                    yield return new KeyValuePair<string, string>("mediumorchid", "#ba55d3");
                    yield return new KeyValuePair<string, string>("mediumpurple", "#9370db");
                    yield return new KeyValuePair<string, string>("mediumseagreen", "#3cb371");
                    yield return new KeyValuePair<string, string>("mediumslateblue", "#7b68ee");
                    yield return new KeyValuePair<string, string>("mediumspringgreen", "#00fa9a");
                    yield return new KeyValuePair<string, string>("mediumturquoise", "#48d1cc");
                    yield return new KeyValuePair<string, string>("mediumvioletred", "#c71585");
                    yield return new KeyValuePair<string, string>("midnightblue", "#191970");
                    yield return new KeyValuePair<string, string>("mintcream", "#f5fffa");
                    yield return new KeyValuePair<string, string>("mistyrose", "#ffe4e1");
                    yield return new KeyValuePair<string, string>("moccasin", "#ffe4b5");
                    yield return new KeyValuePair<string, string>("navajowhite", "#ffdead");
                    yield return new KeyValuePair<string, string>("navy", "#000080");
                    yield return new KeyValuePair<string, string>("oldlace", "#fdf5e6");
                    yield return new KeyValuePair<string, string>("olive", "#808000");
                    yield return new KeyValuePair<string, string>("olivedrab", "#6b8e23");
                    yield return new KeyValuePair<string, string>("orange", "#ffa500");
                    yield return new KeyValuePair<string, string>("orangered", "#ff4500");
                    yield return new KeyValuePair<string, string>("orchid", "#da70d6");
                    yield return new KeyValuePair<string, string>("palegoldenrod", "#eee8aa");
                    yield return new KeyValuePair<string, string>("palegreen", "#98fb98");
                    yield return new KeyValuePair<string, string>("paleturquoise", "#afeeee");
                    yield return new KeyValuePair<string, string>("palevioletred", "#db7093");
                    yield return new KeyValuePair<string, string>("papayawhip", "#ffefd5");
                    yield return new KeyValuePair<string, string>("peachpuff", "#ffdab9");
                    yield return new KeyValuePair<string, string>("peru", "#cd853f");
                    yield return new KeyValuePair<string, string>("pink", "#ffc0cb");
                    yield return new KeyValuePair<string, string>("plum", "#dda0dd");
                    yield return new KeyValuePair<string, string>("powderblue", "#b0e0e6");
                    yield return new KeyValuePair<string, string>("purple", "#800080");
                    yield return new KeyValuePair<string, string>("rebeccapurple", "#663399");
                    yield return new KeyValuePair<string, string>("red", "#ff0000");
                    yield return new KeyValuePair<string, string>("rosybrown", "#bc8f8f");
                    yield return new KeyValuePair<string, string>("royalblue", "#4169e1");
                    yield return new KeyValuePair<string, string>("saddlebrown", "#8b4513");
                    yield return new KeyValuePair<string, string>("salmon", "#fa8072");
                    yield return new KeyValuePair<string, string>("sandybrown", "#f4a460");
                    yield return new KeyValuePair<string, string>("seagreen", "#2e8b57");
                    yield return new KeyValuePair<string, string>("seashell", "#fff5ee");
                    yield return new KeyValuePair<string, string>("sienna", "#a0522d");
                    yield return new KeyValuePair<string, string>("silver", "#c0c0c0");
                    yield return new KeyValuePair<string, string>("skyblue", "#87ceeb");
                    yield return new KeyValuePair<string, string>("slateblue", "#6a5acd");
                    yield return new KeyValuePair<string, string>("slategray", "#708090");
                    yield return new KeyValuePair<string, string>("slategrey", "#708090");
                    yield return new KeyValuePair<string, string>("snow", "#fffafa");
                    yield return new KeyValuePair<string, string>("springgreen", "#00ff7f");
                    yield return new KeyValuePair<string, string>("steelblue", "#4682b4");
                    yield return new KeyValuePair<string, string>("tan", "#d2b48c");
                    yield return new KeyValuePair<string, string>("teal", "#008080");
                    yield return new KeyValuePair<string, string>("thistle", "#d8bfd8");
                    yield return new KeyValuePair<string, string>("tomato", "#ff6347");
                    yield return new KeyValuePair<string, string>("turquoise", "#40e0d0");
                    yield return new KeyValuePair<string, string>("violet", "#ee82ee");
                    yield return new KeyValuePair<string, string>("wheat", "#f5deb3");
                    yield return new KeyValuePair<string, string>("white", "#ffffff");
                    yield return new KeyValuePair<string, string>("whitesmoke", "#f5f5f5");
                    yield return new KeyValuePair<string, string>("yellow", "#ffff00");
                    yield return new KeyValuePair<string, string>("yellowgreen", "#9acd32");
                }
            }
        }
     
        public virtual string IDSelectorPattern { get; set; } = ("\\b((?<=([^\\w\\-\\$]id\\s*=\\s*(['\"]?)(([\\w\\-\\$]*\\s+)*)))[^\\d\\W][\\w\\-\\$]*(?=((\\s+[\\w\\-\\$]*)*\\3)))\\b");
        public virtual string NameSelectorPattern { get; set; } = ("\\b((?<=([^\\w\\$]name\\s*=\\s*(['\"]?)(([\\w\\-\\$]*\\s+)*)))[^\\d\\W][\\w\\-\\$]*(?=((\\s+[\\w\\-\\$]*)*\\3)))\\b");
        public virtual string ClassSelectorPattern { get; set; } = ("\\b((?<=([^\\w\\-\\$]class\\s*=\\s*(['\"]?)(([\\w\\-\\$]*\\s+)*)))[^\\d\\W][\\w\\-\\$]*(?=((\\s+[\\w\\-\\$]*)*\\3)))\\b");
        public virtual string ColorSelectorPattern { get; set; } = ("\\#([abcdef0-9]{6}|[abcdef0-9]{3})((?=[abcdef0-9]{2}\\b)|\\b)");

        public virtual bool IsValidFile(string path)
        {
            return path.ToLower().EndsWith(".svg");
        }

        public virtual SvgDocument AddAttribute(SvgDocument source, string attrName, string value, string toElementNamed = "SVG")
        {
            XmlDocument xmld = new XmlDocument();
            xmld.LoadXml(source.GetXML());
            toElementNamed = toElementNamed.ToUpper();
            foreach (XmlNode ch in xmld.ChildNodes)
                if (ch.NodeType == XmlNodeType.Element && (ch.Name ?? "").ToUpper() == toElementNamed)
                {
                    ((XmlElement)ch).SetAttribute(attrName, (((XmlElement)ch).GetAttribute(attrName) + value).Trim());
                    break;
                }
            return Load(xmld);
        }
        public virtual SvgDocument SetAttribute(SvgDocument source, string attrName, string value, string toElementNamed = "SVG")
        {
            XmlDocument xmld = new XmlDocument();
            xmld.LoadXml(source.GetXML());
            toElementNamed = toElementNamed.ToUpper();
            foreach (XmlNode ch in xmld.ChildNodes)
                if (ch.NodeType == XmlNodeType.Element && (ch.Name ?? "").ToUpper() == toElementNamed)
                {
                    ((XmlElement)ch).SetAttribute(attrName, value);
                    break;
                }
            return Load(xmld);
        }
     
        public override SvgDocument FlipHorizental(SvgDocument source)
        {
            return AddAttribute(source, "transform", " scale(-1,1)"/*, "RDF"*/);
        }
        public override SvgDocument FlipVertical(SvgDocument source)
        {
            return AddAttribute(source, "transform", " scale(1,-1)"/*, "RDF"*/);
        }
        public override SvgDocument Rotate(SvgDocument source, float angle = 180)
        {
            if (angle % 360 == 0) return source;
            return AddAttribute(source, "transform", " rotate(" + angle % 360 + ")");
        }

        public override SvgDocument Change(SvgDocument source, Func<Color, Color> colorProcess)
        {
            var txt = ToString(source);
            var pre = "\\b(?<=((stroke)|(fill)|(color)|(background)|(background\\-color))\\s*\\:\\s*)";
            var post = "\\b";
            foreach (var item in ColorsCodes)
                txt = Regex.Replace(txt, string.Join(item.Key, pre, post), item.Value, RegexOptions.IgnoreCase);
            txt = StringService.Replace(txt, ColorSelectorPattern, item =>
            {
                try
                {
                    string tc = item.Trim().Trim('#');
                    var c = ConvertService.HexaDecimalToColor(tc);
                    //if (c == Color.Empty) return item;
                    string newvalue = ConvertService.ToHexaDecimal(colorProcess(c), tc.Length);
                    return "#" + newvalue;
                }
                catch { }
                return item;
            },false);
            return Load(txt);
        }
     
        public override SvgDocument Combine(params SvgDocument[] sources)
        {
            List<string> ls = new List<string>() { };
            ls.Add(string.Join(Environment.NewLine,"<?xml version='1.0' encoding='utf-8'?>",
                                                    "<!DOCTYPE svg>",
                                                    "<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink'>"//,
                                                    //"\t<metadata>",
                                                    //"\t\t<rdf:RDF xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#'>",
                                                    //"\t\t\t<rdf:Description rdf:about='' xmlns:dc='http://purl.org/dc/elements/1.1/'>",
                                                    //"\t\t\t\t<dc:title><rdf:Seq><rdf:li>MiMFa</rdf:li></rdf:Seq></dc:title>",
                                                    //"\t\t\t\t<dc:creator><rdf:Seq><rdf:li>" + Config.ApplicationName+ "</rdf:li></rdf:Seq></dc:creator>",
                                                    //"\t\t\t\t<dc:url><rdf:Seq><rdf:li>" + Config.URL+ "</rdf:li></rdf:Seq></dc:url>",
                                                    //"\t\t\t</rdf:Description>",
                                                    //"\t\t</rdf:RDF>",
                                                    //"\t</metadata>"
                ));
            int xmln = 0;
            foreach (var item in sources)
            {
                xmln++;
                XmlDocument xmld = new XmlDocument();
                xmld.LoadXml(Standardization(item.GetXML(), "ID-" + xmln + "-{0}", "NAME-" + xmln + "-{0}", "CLASS-" + xmln + "-{0}", true));
                foreach (XmlNode ch in xmld.ChildNodes)
                    if (ch.NodeType == XmlNodeType.Element && (ch.Name ?? "").ToUpper() == "SVG")
                    {
                        int num = 2;
                        for (int i = 0; i < ch.ChildNodes.Count && num > 0; i++)
                        {
                            string name = (ch.ChildNodes[i].Name + "").ToUpper();
                            if (name == "TITLE")
                            {
                                ch.RemoveChild(ch.ChildNodes[i--]);
                                num--;
                            }
                            else if (name == "METADATA")
                            {
                                ch.RemoveChild(ch.ChildNodes[i--]);
                                num--;
                            }   
                        }
                        ls.Add(ch.InnerXml);
                        break;
                    }
            }
            ls.Add("</svg>");
            return Load(string.Join(Environment.NewLine,ls));
        }

        public virtual Image Draw(SvgDocument svgdocument, int width = -1, int height = -1)
        {
            svgdocument.ShapeRendering = SvgShapeRendering.Auto;
            return svgdocument.Draw(width, height);
        }
        public virtual Image Draw(SvgDocument svgdocument, Size size)
        {
            return Draw(svgdocument, size.Width, size.Height);
        }
        public virtual Image Draw(string svgPath, int width = -1, int height = -1)
        {
            Svg.SvgDocument doc = Svg.SvgDocument.Open(svgPath);
            return Draw(doc,width, height);
        }
        public virtual Image Draw(string svgPath, Size size)
        {
            return Draw(svgPath, size.Width, size.Height);
        }

        public virtual bool Save(string path, IEnumerable<string> svgLayers, string title = null, string description = null, params KeyValuePair<string, string>[] metadata)
        {
            return Save(path, (from v in svgLayers select SvgDocument.Open(v)).ToArray(), title, description, metadata) != null;
        }
        public virtual bool Save(string path, string title, string description = null, params KeyValuePair<string, string>[] metadata)
        {
            return Save(path, new SvgDocument[] { SvgDocument.Open(path) }, title, description, metadata) != null;
        }
        public virtual SvgDocument Save(string path, SvgDocument doc, string title = null, string description = null, params KeyValuePair<string, string>[] metadata)
        {
            return Save(path, new SvgDocument[] { doc }, title, description, metadata);
        }
        public virtual SvgDocument Save(string path, IEnumerable<SvgDocument> svgLayers, string title = null, string description = null, params KeyValuePair<string,string>[] metadata)
        {
            var svg = Combine(svgLayers.ToArray());
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(svg.GetXML());
            if(!string.IsNullOrWhiteSpace(description)) XMLService.SetOrPrependElement(doc, doc.DocumentElement, "desc", description);
            if(!string.IsNullOrWhiteSpace(title)) XMLService.SetOrPrependElement(doc, doc.DocumentElement, "title", title);
            svg = Load(doc);
            doc.Save(path);
            return svg;
        }
        public virtual SvgDocument Open(string path)
        {
            return IsValidFile(path) ? SvgDocument.Open(path) : null;
        }

        public virtual SvgDocument Load(XmlDocument svgdocument)
        {
            return svgdocument != null ? SvgDocument.Open(svgdocument) : null;
        }
        public virtual SvgDocument Load(string svgdocument)
        {
            if (string.IsNullOrWhiteSpace(svgdocument)) return null;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(svgdocument);
            return Load(doc);
        }

        public virtual SvgDocument Standardization(SvgDocument svgdocument, string idFormat = "ID-{0}", string nameFormat = "NAME-{0}", string classFormat = "CLASS-{0}", bool deleteUnusableTags = true)
        {
            return Load(Standardization(svgdocument.GetXML(), idFormat, nameFormat, classFormat, deleteUnusableTags));
        }
        public virtual XmlDocument Standardization(XmlDocument svgdocument, string idFormat = "ID-{0}", string nameFormat = "NAME-{0}", string classFormat = "CLASS-{0}", bool deleteUnusableTags = true)
        {
            XmlDocument xmld = new XmlDocument();
            xmld.Load(Standardization(svgdocument.OuterXml, idFormat, nameFormat, classFormat, deleteUnusableTags));
            return xmld;
        }
        public virtual string Standardization(string svgdocument,string idFormat = "ID-{0}", string nameFormat = "NAME-{0}", string classFormat= "CLASS-{0}", bool deleteUnusableTags = true)
        {
            if (deleteUnusableTags)
            {
                svgdocument = Regex.Replace(svgdocument, "(\\<filter(\\b[^\\>]*)?\\>[\\s\\S]*\\<\\/filter\\>)|(\\s+filter: [^\\;]*\\;\\s+)|(\\s+filter=\\\"[^\\\"]*\\\"\\s+)", " ", RegexOptions.IgnoreCase);
            }
            if (!string.IsNullOrEmpty(idFormat)) svgdocument = StringService.ReplaceWithIndices(svgdocument, IDSelectorPattern, "\\b((?<=([^\\w\\-\\$](?:(?:I|i)(?:D|d))\\s*=\\s*(['\"]?)(([\\w\\-\\$]*\\s+)*))){0}(?=((\\s+[\\w\\-\\$]*)*\\3)))|((?<=\\#)\\s*{0})\\b", false,1,idFormat);
            if (!string.IsNullOrEmpty(classFormat)) svgdocument = StringService.ReplaceWithIndices(svgdocument, ClassSelectorPattern, "\\b((?<=([^\\w\\-\\$](?:(?:C|c)(?:L|l)(?:A|a)(?:S|s)(?:S|s))\\s*=\\s*(['\"]?)(([\\w\\-\\$]*\\s+)*))){0}(?=((\\s+[\\w\\-\\$]*)*\\3)))|((?<=\\.)\\s*{0})\\b", false, 1, classFormat);
            if (!string.IsNullOrEmpty(nameFormat)) svgdocument = StringService.ReplaceWithIndices(svgdocument, NameSelectorPattern, false, 1, nameFormat);
            return svgdocument;
        }


        public virtual string ToString(SvgDocument svgdocument)
        {
            return svgdocument.GetXML();
        }
        public virtual string ToString(string path)
        {
            return IsValidFile(path) ? File.ReadAllText(path) : null;
        }
    }
}
