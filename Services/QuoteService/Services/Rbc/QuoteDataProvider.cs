using System;
using System.Net.Http;
using System.Threading.Tasks;
using QuoteService.Interfaces;
using QuoteService.Utils;

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

		public async Task<string> GetQuoteDocumentTextAsync()
		{
			var result = string.Empty;
			var buildUrl = QueryBuilder.Build();

			Uri resourceUri;
			if (!RequestHelper.TryGetUri(buildUrl, out resourceUri))
				return null;

			using (var client = new HttpClient())
			{
				var response = await client.GetAsync(resourceUri);
				if (response.IsSuccessStatusCode)
				{
					result = await response.Content.ReadAsStringAsync();
				}
			}

			return result;
		}
	}
}
