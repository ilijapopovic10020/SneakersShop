using System.Globalization;

namespace SneakersShop.Helpers.Converters
{
    public class Base64ToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64 && !string.IsNullOrWhiteSpace(base64))
            {
                byte[] bytes = System.Convert.FromBase64String(base64);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
