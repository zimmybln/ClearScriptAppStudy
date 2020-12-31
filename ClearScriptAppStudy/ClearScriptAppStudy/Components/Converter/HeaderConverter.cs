using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ClearScriptAppStudy.Types;
using ClearScriptAppStudy.ViewModels;

namespace ClearScriptAppStudy.Components.Converter
{
    public class HeaderConverter : IValueConverter
    {
        public string DefaultValue { get; set; }
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Person item)
            {
                    if (item.Id.Equals(Guid.Empty))
                    {
                        return $"{DefaultValue} *";
                    }

                    return DefaultValue;

            }

            return $"{DefaultValue} ?";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
