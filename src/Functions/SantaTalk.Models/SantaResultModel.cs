using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Models
{
    public class SantaResultModel
    {
        public int ID { get; set; }
        public string KidName { get; set; }
        public string DetectedLanguage { get; set; }
        public string SentimentInterpretation { get; set; }
        public string GiftPrediction { get; set; }
    }
}
