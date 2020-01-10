using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.Media;
using SantaTalk.ViewModels;

namespace SantaTalk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        public CameraPage()
        {
            InitializeComponent();
           
        }

       
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var binding = this.BindingContext as CameraPageViewModel;
           
            binding.StartCamera();
        }
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();

        }
    }
}