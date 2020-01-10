using SantaTalk.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SantaTalk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersMapPage : ContentPage
    {
        public UsersMapPage()
        {
            InitializeComponent();
            LoadDefaultlocation();


        }
        protected  override void OnAppearing()
        {
            base.OnAppearing();
           // LoadDefaultlocation();
            var context = this.BindingContext as UsersMapPageViewModel;
            context.LoadData();
        }

        private async void LoadDefaultlocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);
            var position = new Position(location.Latitude, location.Longitude);
            map.MoveToRegion(
       MapSpan.FromCenterAndRadius(position, new Distance(300d))
    );
        }
    }
}