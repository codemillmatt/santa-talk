using SQLite;
using Xamarin.Forms.Maps;

namespace SantaTalk.Models
{
    public class PinModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        public double Lat { get; set; }

        public double Log { get; set; }

        public string Label { get; set; }

        public string Address { get; set; }

        public int Type { get; set; }
    }
}
