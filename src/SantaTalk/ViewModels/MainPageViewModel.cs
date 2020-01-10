using System;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;

namespace SantaTalk
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            SendLetterCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(KidsName, LetterText));
            });
        }

        string kidsName;
        public string KidsName
        {
            get => kidsName;
            set => SetProperty(ref kidsName, value);
        }

        string letterText = "Dear Santa, I'm Seb. I have five years old and I play football. My Dad loves Azure Cognitives services. I hope you are doing well at the North Pole. I was a very wise child this year. I wish a small blue car this year. Be careful on the roofs. Hugs";
        
        public string LetterText
        {
            get => letterText;
            set => SetProperty(ref letterText, value);
        }

        public ICommand SendLetterCommand { get; }
    }
}
