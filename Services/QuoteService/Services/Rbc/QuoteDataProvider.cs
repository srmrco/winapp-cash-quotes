using System;
using QuoteService.Interfaces;

namespace QuoteService.Services.Rbc
{
	public class QuoteDataProvider : IQuoteDataProvider
	{
		public IQueryBuilder QueryBuilder { get; set; }

		public QuoteDataProvider()
		{
			QueryBuilder = new QueryBuilder();
		}

		public QuoteDataProvider(IQueryBuilder queryBuilder)
		{
			QueryBuilder = queryBuilder;
		}

		public string GetQuoteDocumentText()
		{
			throw new NotImplementedException();
		}
	}
}
