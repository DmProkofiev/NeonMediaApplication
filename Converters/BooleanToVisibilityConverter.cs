using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NeonMediaApplication.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) //Конвертация прямо
        {
            if (!(value is bool boolValue))
                return Visibility.Collapsed;

            bool invert = parameter?.ToString() == "invert";
            if (invert)
                boolValue = !boolValue;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) //Конвертация обратно
        {
            throw new NotImplementedException(); // Заглушка
        }
    }
}
