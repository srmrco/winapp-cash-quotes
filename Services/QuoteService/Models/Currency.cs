using System;

namespace QuoteService.Models
{
	public enum Currency
	{
		EURRUB = 2,
		USDRUB = 3
	}

	public static class CurrencyLabel
	{
		public static string GetLabel(Currency currency)
		{
			switch (currency)
			{
				case Currency.EURRUB:
					return "EUR";
				case Currency.USDRUB:
					return "USD";
				default:
					throw new ArgumentOutOfRangeException("currency", currency, null);
			}
		}

		public static Currency FromLabel(string label)
		{
			switch (label)
			{
				case "EUR":
					return Currency.EURRUB;
				case "USD":
					return Currency.USDRUB;
				default:
					throw new ArgumentException("Not supported Currency label", "label");
			}
		}
	}
}
