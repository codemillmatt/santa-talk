using System;
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
                comments.SentimentInterpretation = "En serio, ¿por qué actuaste así este año? ¿No sabes que siempre estoy mirando? Siempre. Observo.";
                comments.GiftPrediction = "No obtendrás nada y te gustará.";
            }
            else if (results.SentimentScore >= .3 && results.SentimentScore < .66)
            {
                // bad saide of average
                comments.SentimentInterpretation = "Fuiste un buen chico este año. Probablemente deberías haber sido mejor. Sin embargo, lo entiendo, probablemente la culpa es de tu hermano.";
                comments.GiftPrediction = "Si sacas suficientes galletas, podría dejarte algo.";
            }
            else if (results.SentimentScore >= .66 && results.SentimentScore < .95)
            {
                // good side of average
                comments.SentimentInterpretation = "Buen trabajo, chico. Fuiste un buen niño todo el año. ¡El Niño Jesús seguramente pasará por tu casa.!";
                comments.GiftPrediction = "¡Vas a recibir una buena cantidad de regalos este año!";
            }
            else
            {
                // excellent behavior
                comments.SentimentInterpretation = "¡Guauu! ¡Tuviste tu mejor comportamiento! Casi dudo de mí mismo que actuaste tan bien. Pero nunca me equivoco.";
                comments.GiftPrediction = "Los regalos lloverán sobre ti.";
            }

            if (pictureForSantaResults.Age > 14)
            {
                comments.AgeComment = $"¿Ohh pareces de {pictureForSantaResults.Age}, No eres demasiado viejo para un regalo del Niño Jesús?";
            }

            if (pictureForSantaResults.Smile > 0.5 && results.SentimentScore >= .3)
            {
                comments.SmileComment = "Pareces muy feliz en tu foto, Te mereces un regalo.";
            }
            else if (pictureForSantaResults.Smile > 0.5 && results.SentimentScore < .3)
            {
                comments.SmileComment = "Pareces muy feliz en tu foto, sin embargo no mereces un regalo.";
            }
            else if (pictureForSantaResults.Smile > 0 && results.SentimentScore >= .3 && results.SentimentScore < .66)
            {
                comments.SmileComment = "No pareces feliz en tu foto? Quizas no mereces recibir ningun regalo";
            }
            else if (pictureForSantaResults.Smile > 0 && results.SentimentScore >= .66)
            {
                comments.SmileComment = "Animo, no pareces feliz en tu foto!! Pero por tu buen comportamiento este año recibiras un regalo.";
            }
            else if (pictureForSantaResults.Smile > 0 && results.SentimentScore < .3)
            {
                comments.SmileComment = "No pareces feliz en tu foto, tampoco te portaste bien este año, no mereces nigún regalo.";
            }

            return comments;
        }
    }
}
