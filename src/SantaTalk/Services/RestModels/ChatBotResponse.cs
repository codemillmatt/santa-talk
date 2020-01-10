using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SantaTalk.Services.RestModels
{
    public class ChatBotResponse
    {
        [JsonProperty("answers")]
        public List<Answer> Answers { get; set; }
    }
}
