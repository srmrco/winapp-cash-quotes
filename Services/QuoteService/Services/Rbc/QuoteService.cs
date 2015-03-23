using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using QuoteService.Interfaces;
using QuoteService.Models;
using QuoteService.Utils;

namespace QuoteService.Services.Rbc
{
	public class QuoteService : IQuoteService
	{
		public IQuoteDataProvider DataProvider { get; private set; }

		public QuoteService()
		{
			DataProvider = new QuoteDataProvider();
		}

		public QuoteService(IQuoteDataProvider provider)
		{
			DataProvider = provider;
		}

		public IEnumerable<ExchangeData> GetExchangeRates()
		{
			var result = new List<ExchangeData>();
			var dataText = DataProvider.GetQuoteDocumentText();

			if (string.IsNullOrEmpty(dataText))
				return result;

			var doc = new HtmlDocument();
			doc.LoadHtml(dataText);

			var table = doc.DocumentNode.Descendants("table")
				.Where(o => o.Attributes["class"] != null && o.Attributes["class"].Value == "common")
				.FirstOrDefault(o => o.ParentNode.Name == "div" && o.ParentNode.Attributes["id"] != null && o.ParentNode.Attributes["id"].Value == "container_table_rates");
			if (table != null)
			{
				foreach (var row in table.Descendants("tr"))
				{
					var cells = row.Descendants("td").ToList();
					if (cells.Count <= 2)
						continue;

					var dataItem = new ExchangeData();

					dataItem.Name = cells.ExtractTextForHtmlNodeByClass("name");
					dataItem.BuyRate = ConvertHelper.ConvertDecimal(cells.ExtractTextForHtmlNodeByClass("pok"));
					dataItem.SellRate = ConvertHelper.ConvertDecimal(cells.ExtractTextForHtmlNodeByClass("prod"));
					dataItem.AvailableAmount = ConvertHelper.ConvertDecimal(cells.ExtractTextForHtmlNodeByClass("sum"));

					var date = cells.GetHtmlNodeByClass("time");
					if (date.Attributes["title"] != null)
						dataItem.DateCreated = ConvertHelper.ConvertDate(date.Attributes["title"].Value);
					if (!string.IsNullOrEmpty(date.InnerText))
					{
						var time = ConvertHelper.ConvertTime(date.InnerText);
						if (time != TimeSpan.MinValue)
							dataItem.DateCreated = dataItem.DateCreated.AddTicks(time.Ticks);
					}

					result.Add(dataItem);
				}
			}

			return result;
		}
	}
}
