using System.Globalization;

namespace Microsoft.NetConf2021.Maui.Converters
{
    class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;
            var result = string.Empty;

            if (value is string stringValue){
                try
                {
                    var duration = TimeSpan.Parse(stringValue);
                    result = $"{duration.TotalMinutes.ToString("N0")} min";
                }
                finally
                {                    
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
