using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using SantaTalk.Models;
using Xamarin.Essentials;

namespace SantaTalk
{
    public class LetterDeliveryService
    {
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

                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                results = new SantaResults { SentimentScore = -1 };
            }

            return results;
        }
        public async Task<List<FaceInfo>> sendPictureToSanta(MediaFile f)
        {

            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://sgserverless20191227125952.azurewebsites.net/api/Function1?code=5XkValKn15DaoMVvMaz8i6MMBrnZlfAdtDn2yONugCI0P36ByiiYuw==");

                var content = new MultipartFormDataContent();

                byte[] byteArray = File.ReadAllBytes(f.Path);

                var webClient = new WebClient();

                content.Add(new ByteArrayContent(byteArray), "file", "file.jpg");

                request.Content = content;

                var response = await client.SendAsync(request).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                List<FaceInfo> fileInfo = JsonConvert.DeserializeObject<List<FaceInfo>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                return fileInfo;
            }


        }
    }
}
