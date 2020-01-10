using System;
using System.Collections.Generic;
using SantaTalk.ViewModels;
using Xamarin.Forms;

namespace SantaTalk
{
    public partial class LetterHistoryPage : ContentPage
    {
        public LetterHistoryPage()
        {
            BindingContext = new LetterHistoryPageViewModel();
            InitializeComponent();
        }
    }
}
