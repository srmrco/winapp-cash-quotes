using System;
using System.Diagnostics;

namespace QuoteService.Models
{
	[DebuggerDisplay("{Address}")]
	public class ExchangeData 
	{
		public string Name { get; set; }

		public decimal? BuyRate { get; set; }

		public decimal? SellRate { get; set; }

		public string Commission { get; set; }

		public decimal? AvailableAmount { get; set; }

		public string Comments { get; set; }

		public DateTime DateCreated { get; set; }

		public string Address { get; set; }

		public decimal? Distance { get; set; }
	}

	public enum ExchangeDataSortField
	{
		ByBuyRate = 1,
		BySellRate,
		ByDistance
	}
}
