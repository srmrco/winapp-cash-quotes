using System;
using System.Collections.Generic;
using QuoteService.Interfaces;
using QuoteService.Models;

namespace QuoteService.Services.Rbc
{
	public class QuoteService : IQuoteService
	{
		public IQuoteDataProvider DataProvider { get; private set; }

		public QuoteService()
		{
			DataProvider = new QuoteDataProvider();
		}

		public QuoteService(IQuoteDataProvider provider)
		{
			DataProvider = provider;
		}

		public IEnumerable<ExchangeData> GetExchangeRates()
		{
			throw new NotImplementedException();
		}
	}
}
