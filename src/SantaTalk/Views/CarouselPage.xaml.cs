using SantaTalk.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SantaTalk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarouselPage : ContentPage
    {
        public CarouselPage()
        {
            InitializeComponent();
            this.BindingContext = new CarouselPageViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var binding = this.BindingContext as CarouselPageViewModel;
            binding.LoadData();
        }
    }
}