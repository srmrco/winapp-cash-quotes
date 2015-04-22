using System;
using System.Collections.Generic;
using QuoteService.Models;

namespace QuoteService.Utils
{
	public class ExchangeDataSorter : IComparer<ExchangeData>
	{
		public enum SortOrder
		{
			Asc,
			Desc
		}

		public ExchangeDataSortField SortField { get; set; }

		public SortOrder SortDirection { get; set; }

		public ExchangeDataSorter()
		{
			SortField = ExchangeDataSortField.ByBuyRate;
			SortDirection = SortOrder.Desc;
		}

		public ExchangeDataSorter(ExchangeDataSortField field)
			: this()
		{
			SortField = field;
		}

		public ExchangeDataSorter(ExchangeDataSortField field, SortOrder direction)
			: this()
		{
			SortField = field;
			SortDirection = direction;
		}

		public int Compare(ExchangeData x, ExchangeData y)
		{
			int result;
			switch (SortField)
			{
				case ExchangeDataSortField.ByBuyRate:
					if (!x.BuyRate.HasValue && !y.BuyRate.HasValue)
						result = 0;
					else if (!x.BuyRate.HasValue)
						result = -1;
					else if (!y.BuyRate.HasValue)
						result = 1;
					else
						result = x.BuyRate.Value.CompareTo(y.BuyRate.Value);
					break;
				case ExchangeDataSortField.BySellRate:
					if (!x.SellRate.HasValue && !y.SellRate.HasValue)
						result = 0;
					else if (!x.SellRate.HasValue)
						result = -1;
					else if (!y.SellRate.HasValue)
						result = 1;
					else
						result = x.SellRate.Value.CompareTo(y.SellRate.Value);
					break;

				case ExchangeDataSortField.ByDistance:
					if (!x.Distance.HasValue && !y.Distance.HasValue)
						result = 0;
					else if (!x.Distance.HasValue)
						result = -1;
					else if (!y.Distance.HasValue)
						result = 1;
					else
						result = x.Distance.Value.CompareTo(y.Distance.Value);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (SortDirection == SortOrder.Desc)
				result *= -1;

			return result;
		}
	}
}
