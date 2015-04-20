using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CashQuotes.Controls;
using QuoteService.Interfaces;
using QuoteService.Models;
using QuoteService.Services.Rbc;

namespace CashQuotes
{
	/// <summary>
	/// Page that lists exchange rates (quotes) in a gridview
	/// </summary>
	public sealed partial class QuoteListPage : Page
	{
		private IQuoteService _service;

		private IQueryBuilder _queryBuilder;

		private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _delegate;

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
			_queryBuilder = new QueryBuilder { City = City.StPete, Currency = Currency.USDRUB };
			_service = new QuoteService.Services.Rbc.QuoteService(new QuoteDataProvider(_queryBuilder));

			ItemGridView.ItemsSource = _service.GetExchangeRates();
		}

		/// <summary>
		/// We will visualize the data item in asynchronously in multiple phases for improved panning user experience 
		/// of large lists.  In this sample scneario, we will visualize different parts of the data item
		/// in the following order:
		/// 
		///     1) Placeholders (visualized synchronously - Phase 0)
		///     2) Rates (visualized asynchronously - Phase 1)
		///     3) Name and distance (visualized asynchronously - Phase 2)
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ItemGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
		{
			var iv = args.ItemContainer.ContentTemplateRoot as ItemViewer;

			if (args.InRecycleQueue)
			{
				iv.ClearData();
			}
			else if (args.Phase == 0)
			{
				iv.ShowPlaceholder(args.Item as ExchangeData);

				// Register for async callback to visualize Title asynchronously
				args.RegisterUpdateCallback(ContainerContentChangingDelegate);
			}
			else if (args.Phase == 1)
			{
				iv.ShowRates();
				args.RegisterUpdateCallback(ContainerContentChangingDelegate);
			}
			else if (args.Phase == 2)
			{
				iv.ShowName();
				iv.ShowDistance();
			}

			// For imporved performance, set Handled to true since app is visualizing the data item
			args.Handled = true;
		}

		/// <summary>
		/// Managing delegate creation to ensure we instantiate a single instance for optimal performance. 
		/// </summary>
		private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ContainerContentChangingDelegate
		{
			get
			{
				if (_delegate == null)
				{
					_delegate = ItemGridView_ContainerContentChanging;
				}

				return _delegate;
			}
		}

		private void RefreshAppBarButton_Click(object sender, RoutedEventArgs e)
		{
			ItemGridView.ItemsSource = _service.GetExchangeRates();
		}

		private void SortAppBarButton_OnClick(object sender, RoutedEventArgs e)
		{
			// TODO: show a menu to choose how to sort the grid data
		}

		private void ItemsGridView_OnItemClick(object sender, ItemClickEventArgs e)
		{
			var dataItem = e.ClickedItem as ExchangeData;

			if (dataItem != null)
			{
				var mainFrame = MainPage.Current.FindName("MainFrame") as Frame;
				if (mainFrame != null)
					mainFrame.Navigate(typeof(QuoteDetailsPage), dataItem);
			}
		}
	}
}
