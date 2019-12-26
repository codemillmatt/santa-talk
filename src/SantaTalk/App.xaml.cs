using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SantaTalk
{
    public partial class App : Application
    {
        static SantaAnswerDatabase database;
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

        public static SantaAnswerDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new SantaAnswerDatabase();
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
