using System.IO;
using System.Threading.Tasks;
using QuoteService.Interfaces;

namespace Tests.Unit.Utils
{
	public class MockQuoteDataProvider : IQuoteDataProvider 
	{
		public IQueryBuilder QueryBuilder { get; set; }

		public Task<string> GetQuoteDocumentTextAsync()
		{
			var result = Task.Run(() => File.ReadAllText("../../MockData/test-data.html"));

			return result;
		}
	}
}
