using System;
using QuoteService.Interfaces;

namespace Tests.Unit.Utils
{
	public class MockQuoteDataProvider : IQuoteDataProvider 
	{
		public IQueryBuilder QueryBuilder { get; set; }

		public string GetQuoteDocumentText()
		{
			throw new NotImplementedException();
		}
	}
}
