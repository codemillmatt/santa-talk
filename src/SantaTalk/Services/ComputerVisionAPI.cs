using SantaTalk.Models;
using System;
using System.Collections.Generic;
using SantaTalk.Helpers;
using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;

namespace SantaTalk.Services
{
    public class ComputerVisionAPI
    {

        private HttpClientService _httpService;
        private ComputerVisionClient computerVisionClient;

        public ComputerVisionAPI()
        {

            _httpService = new HttpClientService(Common.ComputerVisionEndpoint);

        }
        public async Task<ImageAnalysis> CallComputerVision(string url)
        {
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
                            {
                                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                                VisualFeatureTypes.Objects
                             };
            ImageAnalysis result = null;
            try
            {
                result = await computerVisionClient.AnalyzeImageAsync(url, features);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

       public async Task<CognitiveServiceModel> MakeAnalysisRequest(string imageFilePath)
       // public async Task<string> MakeAnalysisRequest(string imageFilePath)
        {
            CognitiveServiceModel objCognitiveServiceModel=null;
            string contentString = string.Empty;
            try
            {
                HttpClient client = _httpService.GetHttpClient();
                string requestParameters = Common.RequestParameter; ;
                string uri = Common.VisionAnalyzeApiUrl + "?" + requestParameters;
                HttpResponseMessage response;
                byte[] byteData = GetImageAsByteArray(imageFilePath);
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {

                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                }


                contentString = await response.Content.ReadAsStringAsync();
                objCognitiveServiceModel = JsonConvert.DeserializeObject<CognitiveServiceModel>(contentString);
                

            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            return objCognitiveServiceModel;
           // return contentString;
        }
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
