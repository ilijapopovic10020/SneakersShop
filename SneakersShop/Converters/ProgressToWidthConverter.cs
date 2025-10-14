using System.Globalization;

namespace SneakersShop.Converters
{
    public class ProgressToWidthConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double progress && parameter is double totalWidth)
            {
                return totalWidth * progress;
            }
            return 0;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
