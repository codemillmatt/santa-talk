using System;
using System.Threading.Tasks;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SantaTalk.Models;
using Xamarin.Forms.StateSquid;

namespace SantaTalk
{
    public class ResultsPageViewModel : BaseViewModel
    {
        private SantaResults resultsLetter;
        private FaceInfo resultsFace;
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

        string faceImgSource;
        public string FaceImgSource
        {
            get => faceImgSource;
            set => SetProperty(ref faceImgSource, value);
        }
        double faceAge;
        public double FaceAge
        {
            get => faceAge;
            set => SetProperty(ref faceAge, value);
        }
        string faceGender;
        public string FaceGender
        {
            get => faceGender;
            set => SetProperty(ref faceGender, value);
        }
        bool faceNaughty;
        public bool FaceNaughty
        {
            get => faceNaughty;
            set => SetProperty(ref faceNaughty, value);
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
            resultsLetter = await letterService.WriteLetterToSanta(letter);

            if (resultsLetter.SentimentScore == -1)
            {
                resultsLetter.SentimentScore = 1;
                // CurrentState = State.Error;
                return;
            }





        }

        public async Task preparePictureSantaAsync()
        {
            CurrentState = State.Loading;

            var pathToNewFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);


            var fold = System.IO.Directory.CreateDirectory(pathToNewFolder + "/testSanta");


            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "testSanta",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front

            });
            var letterService = new LetterDeliveryService();
            var result = await letterService.sendPictureToSanta(file);
            if (result.Count > 0)
            {
                //prepare to multiple faces
                resultsFace = result[0];
            }
            FaceImgSource = file.Path;
            FaceGender = resultsFace.Gender;
            FaceAge = resultsFace.Age;
            FaceNaughty = resultsFace.smile == 1 ? false : true;
            prepareResult();
            CurrentState = State.Success;
        }

        private void prepareResult()
        {
            var commentsService = new SantasCommentsService();
            var comments = commentsService.MakeGiftDecision(resultsLetter, resultsFace);

            SantasComment = comments.SentimentInterpretation;
            GiftDecision = comments.GiftPrediction;
            DetectedLanguage = resultsLetter.DetectedLanguage;
            CurrentState = State.Success;
        }

    }
}
