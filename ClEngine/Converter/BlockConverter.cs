using System;
using System.Globalization;
using System.Windows.Data;
using ClEngine.CoreLibrary.Asset;

namespace ClEngine.Converter
{
	public class BlockConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
		    return value;
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
		    return string.Concat(value, " ", "Pixel".GetTranslateName());
        }
	}
}