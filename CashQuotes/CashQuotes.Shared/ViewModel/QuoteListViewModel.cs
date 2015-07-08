using System.Collections.ObjectModel;
using System.Linq;
using CashQuotes.DataModel;
using QuoteService.Models;

namespace CashQuotes.ViewModel
{
	public class QuoteListViewModel
	{
		public ObservableCollection<ExchangeDataView> Quotes { get; set; }

		public QuoteListViewModel()
		{
			Quotes = new ObservableCollection<ExchangeDataView>();
		}

		public int? GetViewIndexByCurrency(string currencyLabel)
		{
			var view = Quotes.SingleOrDefault(c => c.Currency == currencyLabel);
			return view == null ? (int?) null : Quotes.IndexOf(view);

		}
	}
}
