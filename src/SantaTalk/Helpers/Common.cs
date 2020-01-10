using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Helpers
{
    static public class Common
    {
        public const string ComputerVisionEndpoint = "https://santachallengevisioservice.cognitiveservices.azure.com/";
        public const string SubscriptionKey = "47ef6b4b9e854e4f98f7e395e4ba2977";

        public static string VisionAnalyzeApiUrl = "vision/v2.1/analyze";
        public static string RequestParameter = "visualFeatures=Categories,Description,Color";

        public const string LogicAppUrl = "https://prod-20.westus2.logic.azure.com:443";
        public const string SendTweetUrl="/workflows/c58c5b5d35ba4f4795299f16361861c5/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=3ayE65Tl699A3abMNQinFMtnbMpruK5iZiCGvxa7cHc";
    }
}
