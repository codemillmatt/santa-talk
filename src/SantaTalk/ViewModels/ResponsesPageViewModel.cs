using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Realms;
using SantaTalk.Models;
using Xamarin.Forms;

namespace SantaTalk.ViewModels
{
    public class ResponsesPageViewModel : INotifyPropertyChanged
    {
        private IEnumerable<SantaResultsData> santaResultsDatas;
        public IEnumerable<SantaResultsData> SantaResultsDatas
        {
            get => santaResultsDatas;
            set
            {
                santaResultsDatas = value;
                OnPropertyChanged("SantaResultsDatas");
            }
        }

        public ResponsesPageViewModel()
        {
            
        }

        public void RefreshData()
        {
            var realm = Realm.GetInstance();

            SantaResultsDatas = realm
                .All<SantaResultsData>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
