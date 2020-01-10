using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Models
{
    public class SantaResults
    {
        public string KidName { get; set; }

        // Text
        public string LetterText { get; set; }
        public string DetectedLanguage { get; set; }
        public double SentimentScore { get; set; }

        // Vision
        public double AdultScore { get; set; }
        public int Faces { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Caption { get; set; }
    }
}
