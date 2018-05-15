using System;
using System.Globalization;
using System.Windows.Data;
using ClEngine.CoreLibrary.Asset;

namespace ClEngine.Converter
{
	public class TranslateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
				return value.GetTranslateName();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}