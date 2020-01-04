using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SantaTalk.Models;
using SantaTalk.Services;
using Xamarin.Essentials;

namespace SantaTalk
{
    public class LetterDeliveryService
    {
        private LettersHistoryService _lettersHistoryService;

        public LetterDeliveryService()
        {
            _lettersHistoryService = new LettersHistoryService();
        }

        //string santaUrl = "{REPLACE WITH YOUR FUNCTION URL}/api/WriteSanta";

        string santaUrl = "http://localhost:7071/api/WriteSanta";
        static HttpClient httpClient = new HttpClient();

        public async Task<SantaResults> WriteLetterToSanta(SantaLetter letter)
        {
            // if we're on the Android emulator, running functions locally, need to swap out the function url
            if (santaUrl.Contains("localhost") && DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DevicePlatform.Android)
            {
                santaUrl = "http://10.0.2.2:7071/api/WriteSanta";
            }

            SantaResults results = null;
            try
            {
                var letterJson = JsonConvert.SerializeObject(letter);

                var httpResponse = await httpClient.PostAsync(santaUrl, new StringContent(letterJson));

                results = JsonConvert.DeserializeObject<SantaResults>(await httpResponse.Content.ReadAsStringAsync());
                
                _lettersHistoryService.SaveLetterAndResults(letter, results);

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
