using System.Collections.Generic;
using QuoteService.Models;

namespace QuoteService.Interfaces
{
	public interface IQuoteService
	{
		IQuoteDataProvider DataProvider { get; }

		IEnumerable<ExchangeData> GetExchangeRates();
	}
}
