using System;
using System.Collections.Generic;
using SantaTalk.Models;
using SantaTalk.Services;

namespace SantaTalk.ViewModels
{
    public class LetterHistoryPageViewModel
    {        
        public IList<Letter> Letters { get; set; }

        public LetterHistoryPageViewModel()
        {
            var lettersService = new LettersHistoryService();
            Letters = lettersService.ListLetters();
        }
    }
}
