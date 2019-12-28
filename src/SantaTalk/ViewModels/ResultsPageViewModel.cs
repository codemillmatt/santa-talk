using System;
using System.Threading.Tasks;
using MvvmHelpers;
using SantaTalk.Models;
using Xamarin.Forms;
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

        Color backgroundColor;
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
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

            BackgroundColor = CalcColor(results.SentimentScore);

            CurrentState = State.Success;
        }

        //  Score | R   | G   | B
        // -------|-----|-----|-----
        //    0   | 1   | 0.5 | 0.5
        //    0.5 | 1   | 1   | 1
        //    1   | 0.5 | 1   | 0.5
        public Color CalcColor(double score)
        {
            if (score < 0.5)
            {
                var x = (0.5 - score) * 2.0;
                var y = x * 0.5 + (1.0 - x) * 1.0;
                return new Color(1.0, y, y);
            }
            else
            {
                var x = (score - 0.5) * 2.0;
                var y = x * 0.5 + (1.0 - x) * 1.0;
                return new Color(y, 1.0, y);
            }
        }
    }
}
