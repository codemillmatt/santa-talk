using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SantaTalk.Helpers;
using SantaTalk.Models;
using Xamarin.Essentials;

namespace SantaTalk
{
    public class LetterDeliveryService
    {
        string basicUrl = Constants.basicUrl;
        string apiUrl = "api/WriteSanta";

        static HttpClient httpClient = new HttpClient();

        public async Task<SantaResults> WriteLetterToSanta(SantaLetter letter)
        {
            var SantaUrl = basicUrl + apiUrl;

            // if we're on the Android emulator, running functions locally, need to swap out the function url
            if (SantaUrl.Contains("localhost") && DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DevicePlatform.Android)
            {
                SantaUrl = "http://10.0.2.2:7071/api/WriteSanta";
            }

            SantaResults results = null;
            try
            {
                var letterJson = JsonConvert.SerializeObject(letter);

                var httpResponse = await httpClient.PostAsync(SantaUrl, new StringContent(letterJson));

                results = JsonConvert.DeserializeObject<SantaResults>(await httpResponse.Content.ReadAsStringAsync());

                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                results = new SantaResults { SentimentScore = -1 };
            }

            return results;
        }
    }
}
