using System;
using System.Threading.Tasks;
using MvvmHelpers;
using Realms;
using SantaTalk.Models;
using Xamarin.Forms.StateSquid;

namespace SantaTalk
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

        public string PictureBase64 { get; set; }
        public string Picture { get; set; }

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

        public string Caption { get; set; }

        public async Task SendLetterToSanta()
        {
            CurrentState = State.Loading;

            var letter = new SantaLetter
            {
                KidName = KidsName,
                LetterText = LetterText,
                PictureBase64 = PictureBase64
            };

            var letterService = new LetterDeliveryService();
            var results = await letterService.WriteLetterToSanta(letter);

            if (results.SentimentScore == -1)
            {
                CurrentState = State.Error;
                return;
            }

            var commentsService = new SantasCommentsService();
            var comments = commentsService.MakeGiftDecision(results);

            SantasComment = comments.SentimentInterpretation;
            GiftDecision = comments.GiftPrediction;
            DetectedLanguage = results.DetectedLanguage;
            Caption = "I see " + results.Caption;

            var realm = Realm.GetInstance();

            // Save to DB
            realm.Write(() =>
            {
                realm.Add(new SantaResultsData
                {
                    KidsName = KidsName,
                    SantasComment = SantasComment,
                    GiftDecision = GiftDecision,
                    DetectedLanguage = DetectedLanguage,
                    Caption = Caption,
                    PicturePath = Picture,
                    Timestamp = DateTime.Now
                });
            });

            CurrentState = State.Success;
        }
    }
}
