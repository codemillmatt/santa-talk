using Newtonsoft.Json;
using SantaTalk.Helpers;
using SantaTalk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SantaTalk.Services
{
    public class UploadPictureService
    {
        string basicUrl = Constants.basicUrl;
        string apiUrl = "api/SendPictureSanta";

        static HttpClient httpClient = new HttpClient();

        public async Task<PictureForSantaResults> UploadPictureForSanta(Stream stream)
        {
            var SantaUrl = basicUrl + apiUrl;

            // if we're on the Android emulator, running functions locally, need to swap out the function url
            if (basicUrl.Contains("localhost") && DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DevicePlatform.Android)
            {
                basicUrl = "http://10.0.2.2:7071/";
            }

            PictureForSantaResults results = null;
            try
            {
                byte[] byteImage;
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    byteImage = ms.ToArray();
                }
                string base64 = Convert.ToBase64String(byteImage);

                var httpResponse = await httpClient.PostAsync(SantaUrl, new StringContent(base64));

                results = JsonConvert.DeserializeObject<PictureForSantaResults>(await httpResponse.Content.ReadAsStringAsync());

                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                results = new PictureForSantaResults { Smile = -1 };
            }

            return results;
        }
    }
}
