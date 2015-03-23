using System;
using System.Net.Http;
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

		public string GetQuoteDocumentText()
		{
			var result = string.Empty;
			var buildUrl = QueryBuilder.Build();

			Uri resourceUri;
			if (!RequestHelper.TryGetUri(buildUrl, out resourceUri))
				return null;

			using (var client = new HttpClient())
			{
				var response = client.GetAsync(resourceUri).Result;
				if (response.IsSuccessStatusCode)
				{
					result = response.Content.ReadAsStringAsync().Result;
				}
			}

			return result;
		}
	}
}
