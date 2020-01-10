using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;
using SantaTalk.Models;
using SantaTalk.Services;
using Xamarin.Forms.StateSquid;

namespace SantaTalk
{
    public class ResultsPageViewModel : BaseViewModel
    {
        string filePath;
        public string FilePath
        {
            get => filePath;
            set => SetProperty(ref filePath, value);
        }

        bool theresFoto;
        public bool TheresFoto
        {
            get => theresFoto;
            set => SetProperty(ref theresFoto, value);
        }

        string emotionPhoto;
        public string EmotionPhoto
        {
            get => emotionPhoto;
            set => SetProperty(ref emotionPhoto, value);
        }

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


        public async Task SendLetterToSanta()
        {
            CurrentState = State.Loading;

            if (!string.IsNullOrEmpty(FilePath))
            {
                TheresFoto = true;
                var emotion = await new PhotoDeliveryService().MakeAnalysisRequest(FilePath);
                var currentEmotion = emotion.FirstOrDefault().FaceAttributes.CurrentEmotion();
                EmotionPhoto = currentEmotion;
                CurrentState = State.Success;
            }

            var letter = new SantaLetter
            {
                KidName = KidsName,
                LetterText = LetterText
            };

            var letterService = new LetterDeliveryService();
            var results = await letterService.WriteLetterToSanta(letter);

            if (results.SentimentScore == -1)
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    CurrentState = State.Error;
                    return;
                }
            }

            var commentsService = new SantasCommentsService();
            var comments = commentsService.MakeGiftDecision(results);

            SantasComment = comments.SentimentInterpretation;
            GiftDecision = comments.GiftPrediction;
            DetectedLanguage = results.DetectedLanguage;

            CurrentState = State.Success;
        }
    }
}
