using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteService.Utils
{
	public class FormatHelper
	{
		private static readonly CultureInfo DefaultCultureInfo = new CultureInfo("ru-RU");

		public static string FormatMoney(decimal money)
		{
			return money.ToString("C", DefaultCultureInfo);
		}

		public static string FormatMoney(decimal? money)
		{
			if (money.HasValue)
				return money.Value.ToString("C", DefaultCultureInfo);

			return string.Empty;
		}
	}
}
