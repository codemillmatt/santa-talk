using System;
using System.Globalization;
using Xamarin.Forms;

namespace SantaTalk.Converters
{
    public class HexToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Color.FromHex(value.ToString());
            }
            catch
            {
                throw new InvalidOperationException("The target must be a valid Hex color"); ;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
