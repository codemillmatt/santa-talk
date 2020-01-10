using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SantaTalk.Services
{
    public class PhotoDeliveryService
    {
        const string subscriptionKey = "4c8b5bdac07d45e58a64734a1a10eff6";
        const string uriBase = "https://brazilsouth.api.cognitive.microsoft.com/face/v1.0/detect";

        public async Task<List<FaceResponse>> MakeAnalysisRequest(string imageFilePath)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            string uri = uriBase + "?" + requestParameters;

            byte[] byteData = GetImageAsByteArray(imageFilePath);

            List<FaceResponse> result;

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await httpClient.PostAsync(uri, content);

                string contentString = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<List<FaceResponse>>(contentString);
            }

            return result;
        }

        private byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}

