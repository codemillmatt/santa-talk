using SantaTalk.Helpers;
using SantaTalk.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SantaTalk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private readonly Helper Helper;
        public MapPage()
        {
            Helper = new Helper();
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await MoveMapToCurrentPositionAsync();

            MapPageViewModel context = BindingContext as MapPageViewModel;
            await context.LoadData();
        }

        private async Task MoveMapToCurrentPositionAsync()
        {
            Location pos = await Helper.OnGetCurrentLocation();

            Position position = new Position(pos.Latitude, pos.Longitude);
             map.MoveToRegion(
             MapSpan.FromCenterAndRadius(position, new Distance(1000d))
            );
        }
    }
}