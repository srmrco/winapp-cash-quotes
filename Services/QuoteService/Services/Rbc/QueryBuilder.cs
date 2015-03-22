using System;
using QuoteService.Interfaces;
using QuoteService.Models;

namespace QuoteService.Services.Rbc
{
	public class QueryBuilder : IQueryBuilder
	{
		public City City { get; set; }

		public Currency Currency { get; set; }

		public string Build()
		{
			throw new NotImplementedException();
		}
	}
}
