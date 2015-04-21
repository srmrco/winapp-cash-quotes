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

		public static string FormatDistance(decimal? distance)
		{
			if (distance.HasValue)
				return string.Format("{0} km", distance.Value.ToString("#.#", DefaultCultureInfo));

			return string.Empty;
		}
	}
}
