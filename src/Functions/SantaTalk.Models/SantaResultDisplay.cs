﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Models
{
    public class SantaResultDisplay
    {        
        public string SentimentInterpretation { get; set; }
        public string GiftPrediction { get; set; }
        public string AgeComment { get; set; }
        public string SmileComment { get; set; }
    }
}
