using System;
using System.Collections.Generic;
using System.Text;

namespace SantaTalk.Models
{
    public class DetectDataResults
    {
        public string KidName { get; set; }

        public string Toy { get; set; }

        public string Age { get; set; }

        public bool HasError { get; set; }
    }
}
