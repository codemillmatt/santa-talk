using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SantaTalk.Models;
using Xamarin.Essentials;

namespace SantaTalk.Services
{
    public class SantaToyService
    {
        static HttpClient httpClient = new HttpClient();

        public async Task<ToyColoringResults> GetToyColoring(string toy)
        {
            if (string.IsNullOrEmpty(toy))
                return null;

            string getToyColoringUrl = "http://localhost:7071/api/GetToyColoring";
            // if we're on the Android emulator, running functions locally, need to swap out the function url
            if (getToyColoringUrl.Contains("localhost") && DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DevicePlatform.Android)
            {
                getToyColoringUrl = "http://10.0.2.2:7071/api/GetToyColoring";
            }

            ToyColoringResults results = null;
            try
            {
                var httpResponse = await httpClient.PostAsync(getToyColoringUrl, new StringContent(toy));

                results = JsonConvert.DeserializeObject<ToyColoringResults>(await httpResponse.Content.ReadAsStringAsync());

                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                results = new ToyColoringResults { HasErrors = false };
            }

            return results;
        }
    }
}
