using System.Collections.ObjectModel;

namespace CashQuotes.DataModel
{
	public class SampleDataSource
	{
		public ObservableCollection<ExchangeDataView> ExchangeQuotes { get; set; }

		public SampleDataSource()
		{
			ExchangeQuotes = new ObservableCollection<ExchangeDataView>();
		}
	}

}
