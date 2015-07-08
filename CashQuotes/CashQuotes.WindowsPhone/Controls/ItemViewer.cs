using Windows.UI.Xaml.Controls;
using CashQuotes.Workflows;
using QuoteService.Utils;

namespace CashQuotes.Controls
{
	public sealed partial class ItemViewer
	{
		public async void ShowDistance()
		{
			Item.Distance = await GeopositionFlow.Instance.GetDistanceToAddress(Item.Address);

			if (Item.Distance.HasValue)
				TxtDistance.Text = FormatHelper.FormatDistance(Item.Distance);
			else
				TxtDistance.ClearValue(TextBlock.TextProperty);
		}
	}
}
