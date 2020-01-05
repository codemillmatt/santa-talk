using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SantaTalk.Services.RestModels
{
    public class Answer
    {
        [JsonProperty("answer")]
        public string Message { get; set; }
    }
}
