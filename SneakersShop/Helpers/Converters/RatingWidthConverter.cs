using SneakersShop.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.Helpers.Converters
{
    public class RatingWidthConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ReviewsViewModel vm && parameter != null)
            {
                if (int.TryParse(parameter.ToString(), out int rating))
                {
                    return vm.GetRatingBarWidth(rating, 150);
                }
            }

            return 0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
