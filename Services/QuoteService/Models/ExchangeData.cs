using System;

namespace QuoteService.Models
{
	public class ExchangeData
	{
		public string Name { get; set; }

		public decimal? BuyRate { get; set; }

		public decimal? SellRate { get; set; }

		public string Commission { get; set; }

		public decimal? AvailableAmount { get; set; }

		public string Comments { get; set; }

		public DateTime DateCreated { get; set; }

		public decimal? Distance { get; set; }
	}
}
