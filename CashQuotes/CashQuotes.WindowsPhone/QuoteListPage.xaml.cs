using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using CashQuotes.Common;
using CashQuotes.Controls;
using CashQuotes.DataModel;
using CashQuotes.ViewModel;
using CashQuotes.Workflows;
using QuoteService.Models;
using QuoteService.Services.Rbc;
using QuoteService.Utils;

namespace CashQuotes
{
	/// <summary>
	/// Page that lists exchange rates (quotes) in a gridview
	/// </summary>
	public sealed partial class QuoteListPage : Page
	{
		private readonly NavigationHelper _navigationHelper;

		private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _delegate;

		private readonly QuoteListViewModel _defaultViewModel = new QuoteListViewModel();

		public QuoteListPage()
		{
			this.InitializeComponent();

			this.NavigationCacheMode = NavigationCacheMode.Required;

			this._navigationHelper = new NavigationHelper(this);
			this._navigationHelper.LoadState += this.NavigationHelper_LoadState;
			this._navigationHelper.SaveState += this.NavigationHelper_SaveState;
		}

		/// <summary>
		/// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
		/// </summary>
		public NavigationHelper NavigationHelper
		{
			get { return this._navigationHelper; }
		}

		/// <summary>
		/// Gets the view model for this <see cref="Page"/>.
		/// </summary>
		public QuoteListViewModel QuoteListViewModel
		{
			get { return this._defaultViewModel; }
		}

		/// <summary>
		/// Populates the page with content passed during navigation. Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="sender">
		/// The source of the event; typically <see cref="NavigationHelper"/>.
		/// </param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
		/// a dictionary of state preserved by this page during an earlier
		/// session. The state will be null the first time a page is visited.</param>
		private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
			await LoadData();
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

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache. Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
		/// <param name="e">Event data that provides an empty dictionary to be populated with
		/// serializable state.</param>
		private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
			// TODO: Save the unique state of the page here.
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

			if (iv == null)
				return;

			if (args.InRecycleQueue)
			{
				iv.ClearData();
			}
			else if (args.Phase == 0)
			{
				iv.DataContext = args.Item as ExchangeData;

				args.RegisterUpdateCallback(ContainerContentChangingDelegate);
			}
			else if (args.Phase == 1)
			{
				iv.ShowDistance();
			}

			// For imporved performance, set Handled to true since app is visualizing the data item
			args.Handled = true;
		}

		/// <summary>
		/// Reload button clicked
		/// </summary>
		private async void RefreshAppBarButton_Click(object sender, RoutedEventArgs e)
		{
			await GeopositionFlow.Instance.UpdateGeopositionAsync();
			await RefreshCurrentPivotView();
		}

		/// <summary>
		/// Item in the ListView is clicked
		/// </summary>
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

		/// <summary>
		/// One of the sorting buttons is clicked
		/// </summary>
		private async void ButtonOnFlyout_Click(object sender, RoutedEventArgs e)
		{
			var selectedItem = sender as ButtonBase;
			ExchangeDataSorter sorter = null;

			if (selectedItem != null)
			{
				switch (selectedItem.Tag.ToString())
				{
					case "buy":
						sorter = new ExchangeDataSorter(ExchangeDataSortField.ByBuyRate, ExchangeDataSorter.SortOrder.Desc);
						break;
					case "sell":
						sorter = new ExchangeDataSorter(ExchangeDataSortField.BySellRate, ExchangeDataSorter.SortOrder.Asc);
						break;
					case "distance":
						sorter = new ExchangeDataSorter(ExchangeDataSortField.ByDistance, ExchangeDataSorter.SortOrder.Asc);
						break;
				}
			}

			// close the flyout first
			var sortFlyout = (Flyout)Resources["SortMenuFlyout"];
			if (sortFlyout != null)
				sortFlyout.Hide();

			// then attempt to update the view
			if (sorter != null)
				await RefreshCurrentPivotView(sorter);
		}

		/// <summary>
		/// Loads all data with a default sorting
		/// </summary>
		private async Task LoadData()
		{
			// USD
			var usdRates = await LoadRates(City.StPete, Currency.USDRUB);
			QuoteListViewModel.Quotes.Insert(0, usdRates);

			// EUR
			var eurRates = await LoadRates(City.StPete, Currency.EURRUB);
			QuoteListViewModel.Quotes.Insert(1, eurRates);
		}

		/// <summary>
		/// Asyncronously load data for specific currency, city and apply custom sorting
		/// </summary>
		private async Task<ExchangeDataView> LoadRates(City city, Currency currency, ExchangeDataSorter sorter = null)
		{
			var queryBuilder = new QueryBuilder { City = city, Currency = currency };
			var service = new QuoteService.Services.Rbc.QuoteService(new QuoteDataProvider(queryBuilder));

			if (sorter == null)
				sorter = ExchangeDataSorter.GetDefault();

			var rates = await service.GetExchangeRatesAsync(sorter);
			return new ExchangeDataView { Currency = CurrencyLabel.GetLabel(currency), Rates = rates.ToList() };
		}

		/// <summary>
		/// Refreshes only the pivot item view that is currently selected
		/// </summary>
		private async Task RefreshCurrentPivotView(ExchangeDataSorter currnetSorter = null)
		{
			var view = QuoteListViewModel.Quotes[Pivot.SelectedIndex];
			var currency = CurrencyLabel.FromLabel(view.Currency);

			var rates = await LoadRates(City.StPete, currency, currnetSorter);
			QuoteListViewModel.Quotes[Pivot.SelectedIndex] = rates;
		}

		#region Flyout menu event methods

		//
		// These methods are used to make the flyout menu look better and more "native"
		// Found out about this approach at http://goo.gl/cT5rNz
		//

		private void SortAppBarButton_OnClick(object sender, RoutedEventArgs e)
		{
			// Ensure we have an app bar
			if (BottomAppBar == null) return;

			// Get the button just clicked
			var sortButton = sender as AppBarButton;
			if (sortButton == null) return;

			// Get the attached flyout
			var sortFlyout = (Flyout)Resources["SortMenuFlyout"];
			if (sortFlyout == null) return;

			var grid = sortFlyout.Content as Grid;
			if (grid == null)
				return;

			grid.Tapped += delegate(object o, TappedRoutedEventArgs args)
			{
				var transparentGrid = args.OriginalSource as Grid;
				if (transparentGrid != null)
				{
					sortFlyout.Hide();
				}
			};

			sortFlyout.ShowAt(BottomAppBar);
		}

		private void FlyoutBase_OnOpening(object sender, object e)
		{
			if (BottomAppBar == null)
				return;

			BottomAppBar.Visibility = Visibility.Collapsed;
		}

		private void FlyoutBase_OnClosed(object sender, object e)
		{
			if (BottomAppBar == null)
				return;

			BottomAppBar.Visibility = Visibility.Visible;
		} 

		#endregion

		#region NavigationHelper registration

		/// <summary>
		/// The methods provided in this section are simply used to allow
		/// NavigationHelper to respond to the page's navigation methods.
		/// <para>
		/// Page specific logic should be placed in event handlers for the  
		/// <see cref="NavigationHelper.LoadState"/>
		/// and <see cref="NavigationHelper.SaveState"/>.
		/// The navigation parameter is available in the LoadState method 
		/// in addition to page state preserved during an earlier session.
		/// </para>
		/// </summary>
		/// <param name="e">Provides data for navigation methods and event
		/// handlers that cannot cancel the navigation request.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			this._navigationHelper.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			this._navigationHelper.OnNavigatedFrom(e);
		}

		#endregion
	}
}
