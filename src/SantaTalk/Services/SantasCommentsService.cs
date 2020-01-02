﻿using System;
using SantaTalk.Models;

namespace SantaTalk
{
    public class SantasCommentsService
    {
        public SantaResultDisplay MakeGiftDecision(SantaResults results)
        {
            // Based on the results - have Santa make some comments on the kids behavior throughout the year
            // and then decide on whether to give them a gift or not.

            SantaResultDisplay comments = new SantaResultDisplay();

            if (results.SentimentScore < .3)
            {
                // very bad behavior
                comments.SentimentInterpretation = "Seriously though, why did you act like that this year? Don't you know I'm always watching? Always. Watching.";
                comments.GiftPrediction = "You'll get nothing and you'll like it.";
            }
            else if (results.SentimentScore >= .3 && results.SentimentScore < .66)
            {
                // bad saide of average
                comments.SentimentInterpretation = "You were kind of a good kid this year. You should have probably been better. I get it though, probably your brother's fault.";
                comments.GiftPrediction = "If you put out enough cookies, I might leave you something.";
            }
            else if (results.SentimentScore >= .66 && results.SentimentScore < .95)
            {
                // good side of average
                comments.SentimentInterpretation = "Nice work there kid. You were a good kid all year long. Santa for sure is stopping at your house!";
                comments.GiftPrediction = "You're going to be getting a good amount of gifts this year!";
            }
            else
            {
                // excellent behavior
                comments.SentimentInterpretation = "Wow! You were on your best behavior ever! I'm almost doubting myself you acted so good. But I'm never wrong.";
                comments.GiftPrediction = "Gifts will rain down upon you.";
            }

            return comments;
        }

        public SantaResultDisplay MakeGiftDecision(SantaResults results, PictureForSantaResults pictureForSantaResults)
        {
            // Based on the results - have Santa make some comments on the kids behavior throughout the year
            // and then decide on whether to give them a gift or not.

            SantaResultDisplay comments = new SantaResultDisplay();

            if (results.SentimentScore < .3)
            {
                // very bad behavior
                comments.SentimentInterpretation = "Seriously though, why did you act like that this year? Don't you know I'm always watching? Always. Watching.";
                comments.GiftPrediction = "You'll get nothing and you'll like it.";
            }
            else if (results.SentimentScore >= .3 && results.SentimentScore < .66)
            {
                // bad saide of average
                comments.SentimentInterpretation = "You were kind of a good kid this year. You should have probably been better. I get it though, probably your brother's fault.";
                comments.GiftPrediction = "If you put out enough cookies, I might leave you something.";
            }
            else if (results.SentimentScore >= .66 && results.SentimentScore < .95)
            {
                // good side of average
                comments.SentimentInterpretation = "Nice work there kid. You were a good kid all year long. Santa for sure is stopping at your house!";
                comments.GiftPrediction = "You're going to be getting a good amount of gifts this year!";
            }
            else
            {
                // excellent behavior
                comments.SentimentInterpretation = "Wow! You were on your best behavior ever! I'm almost doubting myself you acted so good. But I'm never wrong.";
                comments.GiftPrediction = "Gifts will rain down upon you.";
            }

            if (pictureForSantaResults.Age > 14)
            {
                comments.AgeComment = "Aren't you too old for a gift from Santa?";
            }

            if (pictureForSantaResults.Smile > 0.5 && results.SentimentScore >= .3)
            {
                comments.SmileComment = "You're so happy. You deserve to get a gift.";
            }
            else if (pictureForSantaResults.Smile > 0.5 && results.SentimentScore < .3)
            {
                comments.SmileComment = "You're so happy. But you still didn't deserve to get a gift.";
            }
            else if (pictureForSantaResults.Smile > 0 && results.SentimentScore >= .3 && results.SentimentScore < .66)
            {
                comments.SmileComment = "Why aren't you happy? Maybe you don't deserve to get a gift after all.";
            }
            else if (pictureForSantaResults.Smile > 0 && results.SentimentScore >= .66)
            {
                comments.SmileComment = "Why aren't you happy? You will get a gift.";
            }
            else if (pictureForSantaResults.Smile > 0 && results.SentimentScore < .3)
            {
                comments.SmileComment = "Why aren't you happy? You need to change your attitude if you think to get a gift.";
            }

            return comments;
        }
    }
}
