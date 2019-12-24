using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.StateSquid;

namespace SantaTalk
{
    public partial class ResultsPage : ContentPage
    {
        private ResultsPageViewModel vm = new ResultsPageViewModel();

        public ResultsPage(string kidsName, string letterText)
        {
            InitializeComponent();

            BindingContext = vm;

            vm.KidsName = kidsName;
            vm.LetterText = letterText;
            vm.CurrentState = State.Loading;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.SendLetterToSanta();
        }
    }
}
