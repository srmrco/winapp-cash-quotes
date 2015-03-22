using QuoteService.Models;

namespace QuoteService.Interfaces
{
	public interface IQueryBuilder
	{
		City City { get; set; }
		Currency Currency { get; set; }

		string Build();
	}
}
