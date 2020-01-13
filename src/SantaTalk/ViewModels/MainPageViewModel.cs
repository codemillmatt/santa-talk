using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using SantaTalk.Views;
using Xamarin.Forms;

namespace SantaTalk
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand SendLetterCommand { get; private set; }

        public MainPageViewModel()
        {
            SendLetterCommand = new Command(() =>
            NavigateToResultPage().ConfigureAwait(false));
        }

        string kidsName;
        public string KidsName
        {
            get => kidsName;
            set => SetProperty(ref kidsName, value);
        }

        string letterText = "Dear Santa...";
        public string LetterText
        {
            get => letterText;
            set => SetProperty(ref letterText, value);
        }

        async Task NavigateToResultPage()
        {
            var areFieldsValid = await AreFieldsValid(KidsName, LetterText);
            if (!areFieldsValid)
                return;

            await Shell.Current.Navigation.PushAsync(new ResultsPage(KidsName, LetterText), true);
        }

        async Task<bool> AreFieldsValid(string name, string inputText)
        {
            var builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(name))
                builder.AppendLine("Please enter your name.");

            else if (inputText.EndsWith("Dear Santa...", StringComparison.InvariantCultureIgnoreCase))
                builder.AppendLine("Please write something...");

            if (builder.Length != 0)
                await Application.Current.MainPage.DisplayAlert("Invalid Field(s)", builder.ToString(), "Ok");

            return builder.Length == 0;
        }
    }
}
