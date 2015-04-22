using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuoteService.Models;
using QuoteService.Services.Rbc;
using QuoteService.Utils;
using Tests.Unit.Utils;
using QS = QuoteService.Services.Rbc.QuoteService;

namespace Tests.Unit
{
	[TestClass]
	public class RbcQuoteServiceTest
	{
		/// <summary>
		/// Test that service results are parsed into a list of ExchangeData objects
		/// </summary>
		[TestMethod]
		public void TestResultsParsed()
		{
			var queryBuilder = new QueryBuilder { City = City.StPete, Currency = Currency.USDRUB };
			var quoteProvider = new MockQuoteDataProvider { QueryBuilder = queryBuilder };
			var service = new QS(quoteProvider);

			var result = service.GetExchangeRates();

			Assert.IsNotNull(result);

			// this boolean variable is a workaround for some strange issue - 
			// when uncomment the following line, the build will fail with an error 
			//   "The type 'HtmlAgilityPack.HtmlNode' is defined in an assembly that is not referenced."
			// 
			// MAJOR WTF?
			//
			//Assert.IsTrue(result.ToArray().Count() > 0);

			var hasData = false;

			foreach (var item in result)
			{
				Assert.IsNotNull(item.Name);
				Assert.IsNotNull(item.Address);
				Assert.IsTrue(item.BuyRate.HasValue || item.SellRate.HasValue);
				Assert.IsFalse(item.DateCreated == DateTime.MinValue);

				hasData = true; 
			}

			Assert.IsTrue(hasData);
		}


		/// <summary>
		/// Test that service returns results sorter in asc order on BuyRate field
		/// </summary>
		[TestMethod]
		public void TestResultsSorted()
		{
			var queryBuilder = new QueryBuilder { City = City.StPete, Currency = Currency.USDRUB };
			var quoteProvider = new MockQuoteDataProvider { QueryBuilder = queryBuilder };
			var service = new QS(quoteProvider);

			var sorter = new ExchangeDataSorter(ExchangeDataSortField.ByBuyRate);
			var result = service.GetExchangeRates(sorter);

			var previousValue = sorter.SortDirection == ExchangeDataSorter.SortOrder.Asc ? decimal.MinValue : decimal.MaxValue;
			foreach (var item in result)
			{
				if (item.BuyRate.HasValue)
				{
					var order = sorter.SortDirection == ExchangeDataSorter.SortOrder.Asc
						? item.BuyRate.Value.CompareTo(previousValue) > 0
						: item.BuyRate.Value.CompareTo(previousValue) < 0;

					Assert.IsTrue(order);
					previousValue = item.BuyRate.Value;
				}
			}
		}
	}
}
