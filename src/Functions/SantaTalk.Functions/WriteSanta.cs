using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Rest.TransientFaultHandling;
using SantaTalk.Models;

namespace SantaTalk.Functions
{
    public static class WriteSanta
    {
        static TextAnalyticsClient textClient;
        static ComputerVisionClient visionClient;

        static string apiKeyVision = "a7340083b1cc4b17b0ab1d082836a749";
        static string apiKeyText = "dc2c20c584e34dbc8491a2f8d2e8a475";

        static string apiEndpointText = "https://santa-talk-textanalytics.cognitiveservices.azure.com/";
        static string apiEndpointVision = "https://southeastasia.api.cognitive.microsoft.com/";

        static WriteSanta()
        {
            // var keysText = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("APIKeyText"));
            // var keysVision = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("APIKeyVision"));
            
            // textClient = new TextAnalyticsClient(keysText) 
            // { 
            //     Endpoint = Environment.GetEnvironmentVariable("APIEndpointText") 
            // };
            // visionClient = new ComputerVisionClient(keysVision) 
            // {
            //     Endpoint = Environment.GetEnvironmentVariable("APIEndpointVision")
            // };

            var keysText = new ApiKeyServiceClientCredentials(apiKeyText);
            var keysVision = new ApiKeyServiceClientCredentials(apiKeyVision);
            
            textClient = new TextAnalyticsClient(keysText) 
            { 
                Endpoint = apiEndpointText
            };
            visionClient = new ComputerVisionClient(keysVision) 
            {
                Endpoint = apiEndpointVision
            };
        }

        [FunctionName("WriteSanta")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] SantaLetter theLetter,
            ILogger log)
        {
            SantaResults result;

            try
            {
                // Convert base 64 string to byte[]
                byte[] imageBytes = Convert.FromBase64String(theLetter.PictureBase64);

                // Convert byte[] to Stream
                var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                // Creating a list that defines the features to be extracted from the image. 
                List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
                {
                    // VisualFeatureTypes.Categories, 
                    VisualFeatureTypes.Description,
                    VisualFeatureTypes.Faces, 
                    // VisualFeatureTypes.ImageType,
                    // VisualFeatureTypes.Tags, 
                    VisualFeatureTypes.Adult,
                    // VisualFeatureTypes.Color, 
                    // VisualFeatureTypes.Brands,
                    // VisualFeatureTypes.Objects
                };

                // Analyze Image
                var imageAnalysis = await visionClient.AnalyzeImageInStreamAsync(ms, features);
                
                // Get the languages
                var detectedLanguages = await textClient.DetectLanguageAsync(
                    inputText: theLetter.LetterText,
                    countryHint: ""
                );

                if (!string.IsNullOrEmpty(detectedLanguages.ErrorMessage))
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                // Grab the top scoring one
                var theLanguage = detectedLanguages.DetectedLanguages.OrderByDescending(i => i.Score).First();

                // Get the sentiment
                var detectedSentiments = await textClient.SentimentAsync(
                    inputText: theLetter.LetterText,
                    language: theLanguage.Iso6391Name
                );

                if (!string.IsNullOrEmpty(detectedSentiments.ErrorMessage))
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                result = new SantaResults
                {
                    DetectedLanguage = theLanguage.Name,
                    KidName = theLetter.KidName,
                    LetterText = theLetter.LetterText,
                    SentimentScore = detectedSentiments.Score.Value,
                    Caption = imageAnalysis.Description.Captions.FirstOrDefault().Text,
                    AdultScore = imageAnalysis.Adult.AdultScore,
                    Faces = imageAnalysis.Faces.Count,
                    Age = imageAnalysis.Faces.FirstOrDefault().Age,
                    Gender = imageAnalysis.Faces.FirstOrDefault().Gender.Value.ToString()
                };
            } 
            catch (Exception ex)
            {
                log.LogError(ex.ToString());

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(result);
        }
    }
}
