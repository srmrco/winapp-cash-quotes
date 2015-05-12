using Windows.UI.Xaml.Controls;
using CashQuotes.Workflows;
using QuoteService.Utils;

namespace CashQuotes.Controls
{
	public sealed partial class ItemViewer
	{
		public async void ShowDistance()
		{
			_item.Distance = await GeopositionFlow.Instance.GetDistanceToAddress(_item.Address);

			if (_item.Distance.HasValue)
				TxtDistance.Text = FormatHelper.FormatDistance(_item.Distance);
			else
				TxtDistance.ClearValue(TextBlock.TextProperty);
		}
	}
}
