using System;
using SantaTalk.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SantaTalk
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#301536"),
                BarTextColor = Color.Wheat
            };

            MainPage = navPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
