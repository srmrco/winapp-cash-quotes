using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
			var sorter = ExchangeDataSorter.GetDefault();
			var result = GetExchangeRatesAsync(sorter).Result;

			return result;
		}

		public async Task<IEnumerable<ExchangeData>> GetExchangeRatesAsync(ExchangeDataSorter sorter)
		{
			var result = new SortedSet<ExchangeData>(sorter);
			var dataText = await DataProvider.GetQuoteDocumentTextAsync();

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

					var additionalInfo = cells.GetHtmlNodeByClass("info");
					var addressItem = additionalInfo.Descendants("span").FirstOrDefault(s => s.Attributes["id"].Value == "address");
					if (addressItem != null)
						dataItem.Address = addressItem.InnerText;

					// clean data: sometimes they put links into address hidden field
					var htmlEntitiesStart = dataItem.Address.IndexOf("&lt;", StringComparison.Ordinal);
					if (htmlEntitiesStart > 0)
						dataItem.Address = dataItem.Address.Remove(htmlEntitiesStart).Trim();

					// clean data: address may be inside name :(
					dataItem.Name = dataItem.Name.Replace(dataItem.Address, string.Empty);

					// clean data: remove everything inside parentheses
					if (dataItem.Name.IndexOf("(", StringComparison.Ordinal) > 0 && dataItem.Name.IndexOf(")", StringComparison.Ordinal) > 0)
						dataItem.Name = dataItem.Name.Split('(', ')')[1];

					result.Add(dataItem);
				}
			}

			return result;
		}
	}
}
