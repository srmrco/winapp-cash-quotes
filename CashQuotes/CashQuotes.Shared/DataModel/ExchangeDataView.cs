using System.Collections.Generic;
using QuoteService.Models;

namespace CashQuotes.DataModel
{
	public class ExchangeDataView
	{
		public string Currency { get; set; }

		public List<ExchangeData> Rates { get; set; }
	}
}
