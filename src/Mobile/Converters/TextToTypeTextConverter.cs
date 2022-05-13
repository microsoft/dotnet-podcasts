using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.NetConf2021.Maui.Converters;

class TextToTypeTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isHtml = ContainsHTML(value as string);

        return isHtml 
            ? TextType.Html 
            : TextType.Text;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private bool ContainsHTML(string text)
    {
        return !string.IsNullOrWhiteSpace(text) && Regex.IsMatch(text, "<(.|\n)*?>");
    }
}
