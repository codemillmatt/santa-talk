﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using SantaTalk.Models;
using SantaTalk.Services;
using Xamarin.Forms;
using Xamarin.Forms.StateSquid;

namespace SantaTalk
{
    public class ResultsPageViewModel : BaseViewModel
    {
        public ICommand Send_tweet { get; set; }
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
        public ResultsPageViewModel()
        {

            Send_tweet = new Command(SendTweet_Twitter);
        }

        private async void SendTweet_Twitter(object obj)
        {
            var objModel = new TwitterService();
            if (SantasComment!=string.Empty)
            {
               var tweetReply= await objModel.SendTweet(new MessageModel { Message = SantasComment });
                if (!string.IsNullOrEmpty(tweetReply))
                {
                    await App.Current.MainPage.DisplayAlert("Successfull", "Tweet Sent Successfully, Tweet Id " + tweetReply, "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failed", "Tweet Not Sent", "Ok");

                }
            }
        }
    }
}
