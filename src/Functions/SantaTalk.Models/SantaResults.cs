using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Models
{
    public class SantaResults
    {
        public string KidName { get; set; }
        public string LetterText { get; set; }
        public string DetectedLanguage { get; set; }
        public double SentimentScore { get; set; }
    }
}
