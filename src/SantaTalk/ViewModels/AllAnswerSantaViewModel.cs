using MvvmHelpers;
using SantaTalk.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SantaTalk.ViewModels
{
    public class AllAnswerSantaViewModel : BaseViewModel
    {
        private ObservableCollection<SantaResultModel> santaResultDisplays;

        public ObservableCollection<SantaResultModel> SantaResultDisplays
        {
            get => santaResultDisplays;
            set => SetProperty(ref santaResultDisplays, value);
        }

        public AllAnswerSantaViewModel()
        {
            
        }

    }
}
