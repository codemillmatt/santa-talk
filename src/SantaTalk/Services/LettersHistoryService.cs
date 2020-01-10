using System;
using System.Collections.Generic;
using System.IO;
using SantaTalk.Models;
using SQLite;
using SQLitePCL;

namespace SantaTalk.Services
{
    public class LettersHistoryService
    {
        private SQLiteConnection _db;
        private SantasCommentsService _santaResultDisplay;

        public LettersHistoryService()
        {
            _santaResultDisplay = new SantasCommentsService();

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyData.db");
            _db = new SQLiteConnection(databasePath);
            _db.CreateTable<Letter>();
        }

        public IList<Letter> ListLetters()
        {
            return _db.Table<Letter>().ToList();
        }

        public void SaveLetterAndResults(SantaLetter letter, SantaResults results)
        {
            var santaResultDisplay = _santaResultDisplay.MakeGiftDecision(results);
            var santaLetter = new Letter()
            {
                Message = letter.LetterText,
                Receiver = "Santa",
                Sender = letter.KidName
            };

            var resultsLetter = new Letter()
            {
                Message = santaResultDisplay.ToString(),
                Sender = "Santa",
                Receiver = results.KidName
            };

            _db.InsertAll(new List<Letter>() { santaLetter, resultsLetter }, true);
        }
    }
}
