using System.Threading.Tasks;

namespace QuoteService.Interfaces
{
	public interface IQuoteDataProvider
	{
		IQueryBuilder QueryBuilder { get; set; }

		Task<string> GetQuoteDocumentTextAsync();
	}
}
