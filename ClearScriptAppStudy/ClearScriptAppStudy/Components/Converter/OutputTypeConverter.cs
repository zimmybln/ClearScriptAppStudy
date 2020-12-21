using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ClearScriptAppStudy.Services;
using Microsoft.Win32;

namespace ClearScriptAppStudy.Components.Converter
{
    public class OutputTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OutputTypes outputType)
            {
                switch (outputType)
                {
                    case OutputTypes.Error: return System.Windows.Media.Brushes.Red;
                    case OutputTypes.Warning: return System.Windows.Media.Brushes.Yellow;
                    default:
                        return System.Windows.Media.Brushes.Green;
                }
            }

            return System.Windows.Media.Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
