using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Models
{
    public class FaceInfo
    {

        public double Age { get; set; }
        public double smile { get; set; }

        public string Gender { get; set; }

        /// <summary>
        /// Valids values ("Anger","Contempt","Disgust","Fear","Happiness","Neutral","Sadness","Surprise")
        /// </summary>
        public string emotion { get; set; }
    }
}

