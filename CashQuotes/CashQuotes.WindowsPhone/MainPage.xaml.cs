using System;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CashQuotes.Workflows;

namespace CashQuotes
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public static MainPage Current;

		public MainPage()
		{
			this.InitializeComponent();

			this.NavigationCacheMode = NavigationCacheMode.Required;

			// This is a static public property that allows downstream pages to get a handle to the MainPage instance
			// in order to call methods that are in this class.
			Current = this;

			Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (MainFrame.Content == null)
			{
				GeopositionFlow.Instance.UpdateGeopositionAsync();

				if (!MainFrame.Navigate(typeof(QuoteListPage)))
				{
					throw new Exception("Failed to navigate to a page with a list of Quotes");
				}
			}
		}

		void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
		{
			if (MainFrame.CanGoBack)
			{
				MainFrame.GoBack();

				//Indicate the back button press is handled so the app does not exit
				e.Handled = true;
			}
		}
	}
}
