using SantaTalk.Services;
using SantaTalk.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SantaTalk
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //var navPage = new NavigationPage(new AppShell())
            //{
            //    BarBackgroundColor = Color.FromHex("#301536"),
            //    BarTextColor = Color.Wheat
            //};

            //MainPage = navPage;
            MainPage = new AppShell() ;
        }
        private static CognitiveServiceDB database;
        public static CognitiveServiceDB Database
        {
            get
            {
                if (database == null)
                {
                    database = new CognitiveServiceDB();
                }
                return database;
            }
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
