using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Map = Xamarin.Forms.Maps.Map;

namespace SantaTalk
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            Position position = new Position(location.Latitude, location.Longitude);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            Map map = new Map(mapSpan);

            Pin pin = new Pin
            {
                Label = "Santa's destination",
                Type = PinType.Place,
                Position = position
            };

            map.Pins.Add(pin);

            Content = map;
            base.OnAppearing();
        }
    }
}