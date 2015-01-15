using System;
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
                    var Property = UppercaseFirst(property);
                    if (!Property.Equals(property))
                    {
                        var padding = line.TakeWhile(Char.IsWhiteSpace).Count();
                        correctLines.Add(String.Format(@"{0}{1}[JsonProperty(PropertyName = ""{2}"")]", 
                            Environment.NewLine, new String(' ', padding), property));
                        correctLines.Add(line.Replace(String.Format(" {0} ", property), String.Format(" {0} ", Property)));
                    }
                    else
                        correctLines.Add(Environment.NewLine + line);
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
    }
}
