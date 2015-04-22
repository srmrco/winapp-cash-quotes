using System.Collections.Generic;
using QuoteService.Models;
using QuoteService.Utils;

namespace QuoteService.Interfaces
{
	public interface IQuoteService
	{
		IQuoteDataProvider DataProvider { get; }

		IEnumerable<ExchangeData> GetExchangeRates();

		IEnumerable<ExchangeData> GetExchangeRates(ExchangeDataSorter sorter);
	}
}
