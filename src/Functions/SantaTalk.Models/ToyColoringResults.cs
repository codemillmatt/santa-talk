using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Models
{
    public class ToyColoringResults
    {
        public string Toy { get; set; }

        public string ContentUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public bool HasErrors { get; set; }
    }
}
