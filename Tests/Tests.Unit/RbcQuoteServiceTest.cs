using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuoteService.Models;
using QuoteService.Services.Rbc;
using Tests.Unit.Utils;
using QS = QuoteService.Services.Rbc.QuoteService;

namespace Tests.Unit
{
	[TestClass]
	public class RbcQuoteServiceTest
	{
		[TestMethod]
		public void TestResultsParsed()
		{
			var queryBuilder = new QueryBuilder { City = City.StPete, Currency = Currency.USDRUB };
			var quoteProvider = new MockQuoteDataProvider { QueryBuilder = queryBuilder };
			var service = new QS(quoteProvider);

			var result = service.GetExchangeRates();

			var exchangeDatas = result as ExchangeData[] ?? result.ToArray();
			Assert.IsTrue(exchangeDatas.Any());

			foreach (var item in exchangeDatas)
			{
				Assert.IsNotNull(item.Name);
				Assert.IsTrue(item.BuyRate.HasValue || item.SellRate.HasValue);
				Assert.IsFalse(item.DateCreated == DateTime.MinValue);
			}
		}
	}
}
