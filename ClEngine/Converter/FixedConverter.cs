using System;
using System.Globalization;
using System.Windows.Data;

namespace ClEngine.Converter
{
	public class FixedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value + " 块";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}