using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using CashQuotes.Workflows;
using QuoteService.Models;
using QuoteService.Utils;

namespace CashQuotes
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class QuoteDetailsPage : Page
	{
		// A pointer back to the main page.  This is needed if you want to call methods in MainPage such
		// as NotifyUser()
		MainPage rootPage = MainPage.Current;

		public QuoteDetailsPage()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			var quoteData = e.Parameter as ExchangeData;

			if (quoteData == null)
				return;

			BuyPriceBlock.Text = FormatHelper.FormatMoney(quoteData.BuyRate);
			SellPriceBlock.Text = FormatHelper.FormatMoney(quoteData.SellRate);
			DistanceBlock.ClearValue(TextBlock.TextProperty);

			try
			{
				var currentGeoposition = GeopositionFlow.Instance.CurrentGeoposition.Coordinate.Point;

				var targets = await MapLocationFinder.FindLocationsAsync(quoteData.Address, currentGeoposition);
				if (targets.Status != MapLocationFinderStatus.Success || targets.Locations.Count == 0)
				{
					await MapPlace.TrySetViewAsync(currentGeoposition, 16D);
					return;
				}

				//TODO set token
				//MapPlace.MapServiceToken = "token";
				var target = targets.Locations[0].Point;
				await MapPlace.TrySetViewAsync(target, 16D);

				//TODO refactor this to some place closer to QuoteService, ideally I should calculate the distance in 
				// background and have it displayed afterwards
				quoteData.Distance = (decimal?)GeoHelper.DistanceTo(
					currentGeoposition.Position.Latitude, currentGeoposition.Position.Longitude, target.Position.Latitude, target.Position.Longitude);

				DistanceBlock.Text = FormatHelper.FormatDistance(quoteData.Distance);
			}
			catch (Exception)
			{
				//TODO handle geo exception
			}
		}
	}
}
