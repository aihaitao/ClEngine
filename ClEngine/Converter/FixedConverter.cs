using System;
using System.Globalization;
using System.Windows.Data;
using ClEngine.CoreLibrary.Asset;

namespace ClEngine.Converter
{
	public class FixedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
		    return string.Concat(value, " ", "Piece".GetTranslateName());
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
	}
}