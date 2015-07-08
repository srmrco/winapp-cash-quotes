using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Windows.UI.Xaml.Data;

namespace CashQuotes.Converters
{
	public class DistanceConverter : IValueConverter
    {
		private static readonly CultureInfo DefaultCultureInfo = new CultureInfo("ru-RU");

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is decimal)
				return string.Format("{0} km", ((decimal)value).ToString("#.#", DefaultCultureInfo));

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
    }
}
