using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmHelpers;
using SantaTalk.Models;
using SantaTalk.Services;
using Xamarin.Forms;
using Xamarin.Forms.StateSquid;

namespace SantaTalk.ViewModels
{
    public class ResultsPageViewModel : BaseViewModel
    {
        string kidsName;
        public string KidsName
        {
            get => kidsName;
            set => SetProperty(ref kidsName, value);
        }

        string letterText;
        public string LetterText
        {
            get => letterText;
            set => SetProperty(ref letterText, value);
        }

        Stream _photoStream;
        public Stream PhotoStream
        {
            get => _photoStream;
            set => SetProperty(ref _photoStream, value);
        }

        State currentState = State.Loading;
        public State CurrentState
        {
            get => currentState;
            set => SetProperty(ref currentState, value);
        }

        string detectedLanguage;
        public string DetectedLanguage
        {
            get => detectedLanguage;
            set => SetProperty(ref detectedLanguage, value);
        }

        string santasComment;
        public string SantasComment
        {
            get => santasComment;
            set => SetProperty(ref santasComment, value);
        }

        string giftDecision;
        public string GiftDecision
        {
            get => giftDecision;
            set => SetProperty(ref giftDecision, value);
        }

        string ageComment;
        public string AgeComment
        {
            get => ageComment;
            set => SetProperty(ref ageComment, value);
        }

        string smileComment;
        public string SmileComment
        {
            get => smileComment;
            set => SetProperty(ref smileComment, value);
        }

        string kidsNameColor = "#ffffff";
        public string KidsNameColor
        {
            get => kidsNameColor;
            set => SetProperty(ref kidsNameColor, value);
        }

        public ICommand TryAgainCommand { get; }

        public ResultsPageViewModel()
        {
            TryAgainCommand = new Command(async () => await TryAgain(), () => !IsBusy);
        }

        private async Task TryAgain()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
        }

        public async Task SendLetterToSanta()
        {
            CurrentState = State.Loading;

            var letter = new SantaLetter
            {
                KidName = KidsName,
                LetterText = LetterText
            };

            var letterService = new LetterDeliveryService();
            var results = await letterService.WriteLetterToSanta(letter);

            if (results == null)
            {
                await UserDialogs.Instance.AlertAsync("Oh oh. A thunderstorm is affecting communication with the sending of our letter to Santa ..");
                return;
            }

            if (results.SentimentScore == -1)
            {
                CurrentState = State.Error;
                return;
            }

            var uploadService = new UploadPictureService();
            var photoResult = await uploadService.UploadPictureForSanta(PhotoStream);

            var commentsService = new SantasCommentsService();
            var comments = commentsService.MakeGiftDecision(results, photoResult);

            if (comments == null)
            {
                await UserDialogs.Instance.AlertAsync("Oh oh. A thunderstorm is affecting communication with the sending of our letter to Santa ..");
                return;
            }

            if (results.DetectedLanguage.ToLower() == "spanish")
            {
                DetectedLanguage = "español";
            }
            else
            {
                DetectedLanguage = results.DetectedLanguage;
            }

            SantasComment = comments.SentimentInterpretation;
            GiftDecision = comments.GiftPrediction;
            AgeComment = comments.AgeComment;
            SmileComment = comments.SmileComment;

            if (photoResult.Gender == "Male")
            {
                KidsNameColor = "#ccffff";
            }
            else if (photoResult.Gender == "Female")
            {
                KidsNameColor = "#ffcce6";
            }

            CurrentState = State.Success;
        }
    }
}
