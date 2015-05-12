using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using QuoteService.Utils;
using Windows.Services.Maps;

namespace CashQuotes.Workflows
{
	public class GeopositionFlow
	{
		private static GeopositionFlow _instance;

		public static GeopositionFlow Instance
		{
			get { return _instance ?? (_instance = new GeopositionFlow()); }
		}

		public Geoposition CurrentGeoposition { get; private set; }

		public async Task UpdateGeopositionAsync()
		{
			var geolocator = new Geolocator { DesiredAccuracyInMeters = 100 };
			CurrentGeoposition = await geolocator.GetGeopositionAsync();
		}

		public async Task<decimal?> GetDistanceToAddress(string addressLine)
		{
			if (CurrentGeoposition == null)
				await UpdateGeopositionAsync();

			var currentGeoposition = CurrentGeoposition.Coordinate.Point;

			var targets = await MapLocationFinder.FindLocationsAsync(addressLine, currentGeoposition);
			if (targets.Status != MapLocationFinderStatus.Success || targets.Locations.Count == 0)
			{
				return null;
			}

			var target = targets.Locations[0].Point;

			return (decimal?)GeoHelper.DistanceTo(
				currentGeoposition.Position.Latitude, currentGeoposition.Position.Longitude, target.Position.Latitude, target.Position.Longitude);
		}
	}
}
