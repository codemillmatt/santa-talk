using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SantaTalk.Helpers
{
    public class Helper
    {
        private bool IsBusy;
        public Helper()
        {
            IsBusy = false;
        }

        private int Accuracy = (int)GeolocationAccuracy.Default;

        public async Task<Location> OnGetCurrentLocation()
        {
            if (IsBusy)
            {
                return null;
            }

            IsBusy = true;

            string CurrentLocation = string.Empty;
            Location local = null;

            CancellationTokenSource cts = new CancellationTokenSource();

            try
            {
                GeolocationRequest request = new GeolocationRequest((GeolocationAccuracy)Accuracy);
                local = await Geolocation.GetLocationAsync(request, cts.Token);

                CurrentLocation = $"https://www.google.com/maps/search/?api=1&query={local.Latitude},{local.Longitude}";

                //await OpenBrowser(new Uri(gMapsURL2));

                // var local = await CrossGeolocator.Current.GetPositionAsync(new TimeSpan(10000));
                //var map = $"https://maps.googleapis.com/maps/api/staticmap?center={local.Latitude.ToString(CultureInfo.InvariantCulture)},{local.Longitude.ToString(CultureInfo.InvariantCulture)}&zoom=17&size=400x400&maptype=street&markers=color:red%7Clabel:%7C{local.Latitude.ToString(CultureInfo.InvariantCulture)},{local.Longitude.ToString(CultureInfo.InvariantCulture)}&key=AIzaSyAlTv8AoyrMfVwN9zamDLg6XcRfD63YDQg";

            }
            catch (Exception ex)
            {
                CurrentLocation = FormatLocation(null, ex);
            }
            finally
            {
                IsBusy = false;
                cts.Dispose();
            }

            return local;
        }

        private static string FormatLocation(Location location, Exception ex = null)
        {
            string notAvailable = "No Disponible";

            if (location == null)
            {
                return $"Unable to detect location. Exception: {ex?.Message ?? string.Empty}";
            }

            return
                $"Latitude: {location.Latitude}\n" +
                $"Longitude: {location.Longitude}\n" +
                $"Accuracy: {location.Accuracy}\n" +
                $"Altitude: {(location.Altitude.HasValue ? location.Altitude.Value.ToString() : notAvailable)}\n" +
                $"Heading: {(location.Course.HasValue ? location.Course.Value.ToString() : notAvailable)}\n" +
                $"Speed: {(location.Speed.HasValue ? location.Speed.Value.ToString() : notAvailable)}\n" +
                $"Date (UTC): {location.Timestamp:d}\n" +
                $"Time (UTC): {location.Timestamp:T}\n" +
                $"Moking Provider: {location.IsFromMockProvider}";
        }

        private static async Task OpenBrowser(Uri uri)
        {
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

    }
}
