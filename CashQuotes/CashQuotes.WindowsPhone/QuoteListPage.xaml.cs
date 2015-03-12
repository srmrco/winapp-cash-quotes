using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CashQuotes
{
	/// <summary>
	/// Page that lists exchange rates (quotes) in a gridview
	/// </summary>
	public sealed partial class QuoteListPage : Page
	{
		public QuoteListPage()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			// TODO: ItemGridView.ItemsSource = data from service
		}

		private void RefreshAppBarButton_Click(object sender, RoutedEventArgs e)
		{
			// TODO: ItemGridView.ItemsSource = data from service
		}

		private void SortAppBarButton_OnClick(object sender, RoutedEventArgs e)
		{
			// TODO: show a menu to choose how to sort the grid data
		}

	}
}
