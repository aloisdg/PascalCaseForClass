﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PascalCaseForClass.ViewModel;

namespace PascalCaseForClass
{
    // http://wpftutorial.net/ValueConverters.html
    [ValueConversion(typeof(string), typeof(string))]
    public class PascalCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = value as string;
            if (String.IsNullOrWhiteSpace(source))
                return value.ToString();

            var isXml = source.StartsWith("0");
            return ConvertToPascalCase(source.Substring(1), isXml);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        // [XmlRoot(ElementName = "g")]
        private static string ConvertToPascalCase(string source, bool isXml)
        {
            var correctLines = new Collection<string>();
            var lines = source.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (!String.IsNullOrWhiteSpace(line) && line.EndsWith(" { get; set; }"))
                {
                    var tab = line.Remove(line.Length - 14).Split(' ');
                    var property = tab[tab.Length - 1];
                    var Property = ReplaceUnderscore(property);
                    //if (!Property.Equals(property))
                    //{
                    var padding = line.TakeWhile(Char.IsWhiteSpace).Count();
                    var attribute =
                        String.Format(isXml ? @"[XmlElement(""{0}"")]" : @"[JsonProperty(PropertyName = ""{0}"")]",
                            property);
                    correctLines.Add(String.Format(@"{0}{1}{2}", Environment.NewLine, new String(' ', padding),
                        attribute));
                    correctLines.Add(line.Replace(String.Format(" {0} ", property), String.Format(" {0} ", Property)));
                    //}
                    //else
                    //{
                    //    //var b = index > 0 && lines[index - 1].Equals("{");
                    //    correctLines.Add(Environment.NewLine + line);
                    //}
                }
                else
                    correctLines.Add(line);
            }
            return String.Join(Environment.NewLine, correctLines);
        }

        // http://www.dotnetperls.com/uppercase-first-letter
        static string UppercaseFirst(string s)
        {
            var a = s.ToCharArray();
            a[0] = Char.ToUpper(a[0]);
            return new String(a);
        }

        private static string ReplaceUnderscore(string source, string result = "")
        {
            source = UppercaseFirst(source);
            var index = source.IndexOf('_');
            return (index != -1)
                ? ReplaceUnderscore(
                    source.Substring(index + 1),
                    result + source.Substring(0, index))
                : result + source;
        }
    }
}
