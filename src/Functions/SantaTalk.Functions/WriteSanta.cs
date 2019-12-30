namespace SantaTalk.Functions
{
    public static class WriteSanta
    {
        static TextAnalyticsClient textClient;

        static WriteSanta()
        {
            var keys = new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("2fb*****************a6ef3a6"));

            textClient = new TextAnalyticsClient(keys) { Endpoint = Environment.GetEnvironmentVariable("https://eastasia.api.cognitive.microsoft.com/") };
        }

        [FunctionName("WriteSanta")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] SantaLetter theLetter,ILogger log)
        {
            SantaResults result;

            try
            {
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
                    SentimentScore = detectedSentiments.Score.Value
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
