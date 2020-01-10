using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Text;

namespace SantaTalk.Functions
{
    public static class ScanSanta
    {
        static ComputerVisionClient visionClient;
        private const int numberOfCharsInOperationId = 36;

        static ScanSanta()
        {
            var keys = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("ComputerVisionAPIKey"));

            visionClient = new ComputerVisionClient(keys) { Endpoint = Environment.GetEnvironmentVariable("ComputerVisionAPIEndpoint") };
        }

        [FunctionName("ScanSanta")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] Stream image,
            ILogger log)
        {
            var mode = TextRecognitionMode.Handwritten;
            var text = string.Empty;

            try
            {
                var result = await visionClient.RecognizeTextInStreamAsync(image, mode);
                text = await GetTextAsync(result.OperationLocation);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(text);
        }

        private static async Task<string> GetTextAsync(string operationLocation)
        {
            var operationId = operationLocation.Substring(
                operationLocation.Length - numberOfCharsInOperationId);

            var result = await visionClient.GetTextOperationResultAsync(operationId);

            int i = 0;
            int maxRetries = 10;

            while ((result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries)
            {
                await Task.Delay(1000);
                result = await visionClient.GetTextOperationResultAsync(operationId);
            }

            var sb = new StringBuilder();

            foreach (var line in result.RecognitionResult.Lines)
            {
                foreach (var word in line.Words)
                {
                    sb.Append(word.Text);
                    sb.Append(" ");
                }

                sb.Append("\r\n");
            }

            return sb.ToString();
        }
    }
}