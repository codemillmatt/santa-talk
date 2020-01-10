using Xamarin.Forms.Maps;

namespace SantaTalk.Models
{
    public class MapModel
    {
        public Position Position { get; set; }

        public string Label { get; set; }

        public string Address { get; set; }

        public PinType Type { get; set; }
    }
}
