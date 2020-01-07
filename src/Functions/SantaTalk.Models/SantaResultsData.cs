using System;
using Realms;

namespace SantaTalk.Models
{
    public class SantaResultsData : RealmObject
    {
        public string KidsName { get; set; }
        public string DetectedLanguage { get; set; }
        public string SantasComment { get; set; }
        public string GiftDecision { get; set; }
        public string Caption { get; set; }
        public string PicturePath { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
