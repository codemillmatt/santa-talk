using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Models
{
    public class Word
    {
        public List<int> BoundingBox { get; set; }
        public string Text { get; set; }
        public string Confidence { get; set; }
    }

    public class Line
    {
        public List<int> BoundingBox { get; set; }
        public string Text { get; set; }
        public List<Word> Words { get; set; }
    }

    public class RecognitionResult
    {
        public int Page { get; set; }
        public double ClockwiseOrientation { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Unit { get; set; }
        public List<Line> Lines { get; set; }
    }

    public class RootObject
    {
        public string Status { get; set; }

        [JsonProperty("recognitionResults")]
        public List<RecognitionResult> RecognitionResults { get; set; }
    }
}
