using System.Collections.Generic;
using System.Threading.Tasks;
using QuoteService.Models;
using QuoteService.Utils;

namespace QuoteService.Interfaces
{
	public interface IQuoteService
	{
		IQuoteDataProvider DataProvider { get; }

		IEnumerable<ExchangeData> GetExchangeRates();

		Task<IEnumerable<ExchangeData>> GetExchangeRatesAsync(ExchangeDataSorter sorter);
	}
}
