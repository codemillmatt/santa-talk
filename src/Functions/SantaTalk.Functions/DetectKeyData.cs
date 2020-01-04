using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using SantaTalk.Models;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace SantaTalk.Functions
{
    public static class DetectKeyData
    {
        static TextAnalyticsClient textClient;

        static DetectKeyData()
        {
            var keys = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("APIKey"));

            textClient = new TextAnalyticsClient(keys) { Endpoint = Environment.GetEnvironmentVariable("APIEndpoint") };
        }

        [FunctionName("DetectKeyData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] string phrase,
            ILogger log)
        {
            try
            {
                // Get the key phrases
                var detectedWords = await textClient.KeyPhrasesAsync(phrase);

                if (!string.IsNullOrEmpty(detectedWords.ErrorMessage))
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                // Get the entities
                var entitiesResults = await textClient.EntitiesAsync(phrase);

                // Detect hobbies or toy...by deleting entities from keyPrases
                var cleanKeyPhrases = detectedWords.KeyPhrases.Where(key => !entitiesResults.Entities.Any(e => e.Name.Contains(key) || key.Contains(e.Name)));

                // Return result
                var result = new DetectDataResults
                {
                    KidName = entitiesResults?.Entities?.FirstOrDefault(entity => entity.Type == "Person")?.Name,
                    Age = entitiesResults?.Entities?.FirstOrDefault(entity => entity.Type == "Quantity" && entity.SubType == "Age")?.Name,
                    Toy = cleanKeyPhrases?.FirstOrDefault(),
                    HasError = false
                };

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
