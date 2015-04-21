using Windows.UI.Xaml.Controls;
using QuoteService.Models;
using QuoteService.Utils;

namespace CashQuotes.Controls
{
    public sealed partial class ItemViewer : UserControl
    {
		protected ExchangeData _item;

        public ItemViewer()
        {
            this.InitializeComponent();
        }

		/// <summary>
		/// This method visualizes the placeholder state of the data item. When 
		/// showing a placehlder, we set the opacity of other elements to zero
		/// so that stale data is not visible to the end user.  Note that we use
		/// Grid's background color as the placeholder background.
		/// </summary>
		/// <param name="item"></param>
		public void ShowPlaceholder(ExchangeData item)
		{
			_item = item;
			TxtName.Opacity = 0;
			TxtBuyRate.Opacity = 0;
			TxtSellRate.Opacity = 0;
			TxtAddress.Opacity = 0;
		}

		/// <summary>
		/// Visualize the Title by updating the TextBlock for Title and setting Opacity
		/// to 1.
		/// </summary>
		public void ShowNameAndAddress()
		{
			TxtName.Text = _item.Name;
			TxtName.Opacity = 1;

			TxtAddress.Text = _item.Address;
			TxtAddress.Opacity = 1;
		}

		/// <summary>
		/// Visualize category information by updating the correct TextBlock and 
		/// setting Opacity to 1.
		/// </summary>
		public void ShowRates()
		{
			if (_item.BuyRate.HasValue)
			{
				TxtBuyRate.Text = FormatHelper.FormatMoney(_item.BuyRate.Value);
				TxtBuyRate.Opacity = 1;
			}

			if (_item.SellRate.HasValue)
			{
				TxtSellRate.Text = FormatHelper.FormatMoney(_item.SellRate.Value);
				TxtSellRate.Opacity = 1;
			}
		}

		/// <summary>
		/// Drop all refrences to the data item
		/// </summary>
		public void ClearData()
		{
			_item = null;
			TxtName.ClearValue(TextBlock.TextProperty);
			TxtAddress.ClearValue(TextBlock.TextProperty);
			TxtBuyRate.ClearValue(TextBlock.TextProperty);
			TxtSellRate.ClearValue(TextBlock.TextProperty);
			TxtDistance.ClearValue(TextBlock.TextProperty);
		}
    }
}
