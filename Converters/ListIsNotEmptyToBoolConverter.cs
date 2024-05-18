using System.Globalization;

namespace FlickrExplorer.Converters
{
    /// <summary>
    /// Converte conteggio elementi lista in booleano
    /// Se maggiore di 1, converte in true, viceversa in false
    /// </summary>
    public class ListIsNotEmptyToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            System.Collections.IList list = value as System.Collections.IList;
            return list.Count > 0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
