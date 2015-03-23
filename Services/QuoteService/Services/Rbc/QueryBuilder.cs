using System;
using QuoteService.Interfaces;
using QuoteService.Models;

namespace QuoteService.Services.Rbc
{
	public class QueryBuilder : IQueryBuilder
	{
		private const string DEFAULT_SORT_BY = "DT_LAST_PUBLICATE";

		private const string DEFAULT_SORT_DIRECTION = "DESC";

		private const string BaseURL = "http://quote.rbc.ru/cgi-bin/front/content/cash_currency_rates/";

		public City City { get; set; }

		public Currency Currency { get; set; }

		public string Build()
		{
			var rand = new Random();

			var url = string.Format("?sortf={0}&sortd={1}&city={2}&currency={3}&summa=&period=60&pagerLimiter=70&pageNumber=1&r=0.0000{4}",
				DEFAULT_SORT_BY,
				DEFAULT_SORT_DIRECTION,
				(int)City,
				(int)Currency,
				rand.Next(893933499)
			);

			return BaseURL + url;
		}
	}
}
