using System.IO;
using QuoteService.Interfaces;

namespace Tests.Unit.Utils
{
	public class MockQuoteDataProvider : IQuoteDataProvider 
	{
		public IQueryBuilder QueryBuilder { get; set; }

		public string GetQuoteDocumentText()
		{
			var result = File.ReadAllText("../../MockData/test-data.html");

			return result;
		}
	}
}
