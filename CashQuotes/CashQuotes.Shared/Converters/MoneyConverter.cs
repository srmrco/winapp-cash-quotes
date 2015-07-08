using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace CashQuotes.Converters
{
	public class MoneyConverter : IValueConverter
    {
		private static readonly CultureInfo DefaultCultureInfo = new CultureInfo("ru-RU");

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is decimal)
				return ((decimal)value).ToString("C", DefaultCultureInfo);

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
    }
}
