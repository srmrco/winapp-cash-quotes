using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CashQuotes.Common;
using CashQuotes.DataModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using QuoteService.Models;
using QuoteService.Services.Rbc;
using QuoteService.Utils;
using Tests.PhoneUnit.Utils;
using QS = QuoteService.Services.Rbc.QuoteService;

namespace Tests.PhoneUnit
{
	[TestClass]
	public class ListPageTests
	{
		[TestMethod]
		public void TestCreateJsonSampleData()
		{
			//TODO: remove this or decide how to use it

			var queryBuilder = new QueryBuilder { City = City.StPete, Currency = Currency.USDRUB };
			var quoteProvider = new MockQuoteDataProvider { QueryBuilder = queryBuilder };
			var service = new QS(quoteProvider);

			var sorter = new ExchangeDataSorter(ExchangeDataSortField.ByBuyRate);
			var result = service.GetExchangeRatesAsync(sorter).Result;

			var map = new SampleDataSource();//new Dictionary<string, IEnumerable<ExchangeData>> {{"USD", result}, {"EUR", result}};
//
//			var exchangeDatas = result as ExchangeData[] ?? result.ToArray();
//			var usd = new ExchangeDataView { Currency = "USD", Rates = exchangeDatas.ToList() };
//			var eur = new ExchangeDataView { Currency = "EUR", Rates = exchangeDatas.ToList() };
//
//			map.ExchangeQuotes.Add(usd);
//			map.ExchangeQuotes.Add(eur);
//
//			var seralized = Newtonsoft.Json.JsonConvert.SerializeObject(map);
//
//			Assert.IsNotNull(seralized);

		}
	}
}
