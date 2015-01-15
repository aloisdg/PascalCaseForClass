using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PascalCaseForClass
{
    // http://wpftutorial.net/ValueConverters.html
    [ValueConversion(typeof(string), typeof(string))]
    public class PascalCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = value as string;
            return !String.IsNullOrEmpty(source)
                ? ConvertToPascalCase(source, culture)
                : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        // http://stackoverflow.com/questions/1206019/converting-string-to-title-case-in-c-sharp
        private static string ConvertToPascalCase(string source, CultureInfo culture)
        {
            var correctLines = new Collection<string>();
            var lines = source.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (!String.IsNullOrWhiteSpace(line) && line.EndsWith(" { get; set; }"))
                {
                    var tab = line.Remove(line.Length - 14).Split(' ');
                    var property = tab[tab.Length - 1];
                    correctLines.Add(line.Replace(
                        String.Format(" {0} ", property),
                        String.Format(" {0} ", culture.TextInfo.ToTitleCase(property))
                        ));
                }
                else
                    correctLines.Add(line);
            }
            return String.Join(Environment.NewLine, correctLines);
        }
    }
}
