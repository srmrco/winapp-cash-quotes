using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteService.Utils
{
	public class ConvertHelper
	{
		public static readonly CultureInfo DefaultCultureInfo = new CultureInfo("ru-RU");

		public static decimal? ConvertDecimal(string text)
		{
			if (string.IsNullOrEmpty(text))
				return null;

			return Decimal.Parse(text, new NumberFormatInfo { NumberDecimalSeparator = "." });
		}

		public static DateTime ConvertDate(string date)
		{
			var result = DateTime.MinValue;
			if (string.IsNullOrEmpty(date))
				return result;

			DateTime.TryParse(date, DefaultCultureInfo, DateTimeStyles.None, out result);

			return result;
		}

		public static TimeSpan ConvertTime(string time)
		{
			var result = TimeSpan.MinValue;
			if (string.IsNullOrEmpty(time))
				return result;

			TimeSpan.TryParse(time, DefaultCultureInfo, out result);

			return result;
		}

	}
}
