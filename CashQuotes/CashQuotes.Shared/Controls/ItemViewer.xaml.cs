using Windows.UI.Xaml.Controls;
using QuoteService.Models;

namespace CashQuotes.Controls
{
	public sealed partial class ItemViewer
	{
		private ExchangeData Item
		{
			get { return DataContext as ExchangeData; }
		}

		public ItemViewer()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Drop all refrences to the data item
		/// </summary>
		public void ClearData()
		{
			DataContext = null;
			TxtName.ClearValue(TextBlock.TextProperty);
			TxtAddress.ClearValue(TextBlock.TextProperty);
			TxtBuyRate.ClearValue(TextBlock.TextProperty);
			TxtSellRate.ClearValue(TextBlock.TextProperty);
			TxtDistance.ClearValue(TextBlock.TextProperty);
		}
	}
}
