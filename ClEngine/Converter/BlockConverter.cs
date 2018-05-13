using System;
using System.Globalization;
using System.Windows.Data;

namespace ClEngine.Converter
{
	public class BlockConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value + " 像素点";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}