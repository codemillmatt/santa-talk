using System;
using System.Windows.Input;
using MvvmHelpers;
using SantaTalk.Helper;
using Xamarin.Forms;

namespace SantaTalk
{
    public class MainPageViewModel : BaseViewModel
    {
        UserDB message = new UserDB();
        public MainPageViewModel()
        {
            SendLetterCommand = new Command(async () =>
            {
                _ = message.AddMessage(new Models.SantaLetter
                {
                    KidName = this.KidsName,
                    LetterText = this.LetterText
                });
                await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(KidsName, LetterText));
            });
        }

        private string kidsName;
        public string KidsName
        {
            get => kidsName;
            set => SetProperty(ref kidsName, value);
        }

        internal string letterText = "Dear Santa...";
        public string LetterText
        {
            get => letterText;
            set => SetProperty(ref letterText, value);
        }

        public ICommand SendLetterCommand { get; }
    }
}
