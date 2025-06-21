using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.Helpers.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;
            return false;
        }

        // ConvertBack: isto inverzija (ako ti treba dvosmerno bindovanje)
        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;
            return false;
        }
    }
}
