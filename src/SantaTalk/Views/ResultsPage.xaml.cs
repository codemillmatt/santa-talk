using SantaTalk.ViewModels;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.StateSquid;
using Xamarin.Forms.Xaml;

namespace SantaTalk
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : ContentPage
    {
        private ResultsPageViewModel vm = new ResultsPageViewModel();

        public ResultsPage(string kidsName, string letterText, Stream stream)
        {
            InitializeComponent();

            BindingContext = vm;

            vm.KidsName = kidsName;
            vm.LetterText = letterText;
            vm.CurrentState = State.Loading;
            vm.PhotoStream = stream;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.SendLetterToSanta();
        }
    }
}