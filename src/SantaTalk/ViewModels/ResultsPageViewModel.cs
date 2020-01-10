﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using SantaTalk.Models;
using SantaTalk.Services;
using Xamarin.Essentials;
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

        string detectedAge;
        public string DetectedAge
        {
            get => detectedAge;
            set => SetProperty(ref detectedAge, value);
        }

        string toyComment;
        public string ToyComment
        {
            get => toyComment;
            set => SetProperty(ref toyComment, value);
        }

        string toyColoringContentUrl;
        public string ToyColoringContentUrl
        {
            get => toyColoringContentUrl;
            set => SetProperty(ref toyColoringContentUrl, value);
        }

        string toyColoringThumbnailUrl;
        public string ToyColoringThumbnailUrl
        {
            get => toyColoringThumbnailUrl;
            set => SetProperty(ref toyColoringThumbnailUrl, value);
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

            CurrentState = State.Success;
        }

        public async Task SendLetterToSantaAndGetColoring()
        {
            CurrentState = State.Loading;

            var letterService = new LetterDeliveryService();
            var results = await letterService.WriteLetterToSantaAndDetectData(LetterText);

            if (results.HasError)
            {
                CurrentState = State.Error;
                return;
            }

            DetectedAge = (!string.IsNullOrEmpty(results.Age)) ? $"({results.Age})" : "";
            KidsName = results.KidName;
            

            if (string.IsNullOrEmpty(results.Toy))
                return;

            var santaToyService = new SantaToyService();
            var toycoloringResults = await santaToyService.GetToyColoring(results.Toy);

            if (!results.HasError)
            {
                ToyComment = $"Follow this link to download a drawing corresponding to {toycoloringResults.Toy} :";
                ToyColoringContentUrl = toycoloringResults.ContentUrl;
                ToyColoringThumbnailUrl = toycoloringResults.ThumbnailUrl;
            }

            CurrentState = State.Success;
        }

        public ICommand ClickCommand => new Command<string>((url) =>
        {
            Launcher.OpenAsync(new System.Uri(url));
        });
    }
}
