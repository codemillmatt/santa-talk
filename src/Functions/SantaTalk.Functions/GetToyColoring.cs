using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using System.Linq;
using SantaTalk.Models;

namespace SantaTalk.Functions
{
    public static class GetToyColoring
    {
        static ImageSearchClient client;

        static GetToyColoring()
        {
            var keys = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("APIKeyBingSearch"));

            client = new ImageSearchClient(keys);
        }

        [FunctionName("GetToyColoring")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] string toy,
            ILogger log)
        {
            try
            {
                if (string.IsNullOrEmpty(toy))
                    return new StatusCodeResult(StatusCodes.Status204NoContent);

                string searchTerm = $"coloring {toy}";

                // make the search request to the Bing Image API, and get the results"
                Images imageResults = client.Images.SearchAsync(query: searchTerm).Result;

                ToyColoringResults result = new ToyColoringResults
                {
                    Toy = toy,
                    ContentUrl = imageResults?.Value.First().ContentUrl,
                    ThumbnailUrl = imageResults?.Value.First().ThumbnailUrl,
                    HasErrors = false
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
