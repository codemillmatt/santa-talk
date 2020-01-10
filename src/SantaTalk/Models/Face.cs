using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SantaTalk.Models
{
    public class Face
    {
        [JsonProperty("smile")]
        public double Smile { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("age")]
        public double Age { get; set; }

        [JsonProperty("emotion")]
        public Dictionary<string, double> Emotions;

        public string CurrentEmotion()
        {
            var emotions = Emotions.OrderByDescending(x => x.Value);
            return emotions.FirstOrDefault().Key;
        }
    }
}
