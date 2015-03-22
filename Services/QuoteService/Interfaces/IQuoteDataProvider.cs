namespace QuoteService.Interfaces
{
	public interface IQuoteDataProvider
	{
		IQueryBuilder QueryBuilder { get; set; }

		string GetQuoteDocumentText();
	}
}
