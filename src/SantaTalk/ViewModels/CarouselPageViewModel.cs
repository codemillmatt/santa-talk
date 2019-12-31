using MvvmHelpers;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SantaTalk.Models;
using SantaTalk.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace SantaTalk.ViewModels
{
    class CarouselPageViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<CognitiveServiceModel> objModel;
        public ObservableCollection<CognitiveServiceModel> ObjModel
        {
            get
            {
                return objModel;
            }
            set
            {
                if (objModel!=value)
                {
                    objModel = value;
                   // OnPropertyChanged(new PropertyChangedEventArgs("ObjModel"));

                }
            }

        }

        public CarouselPageViewModel()
        {


        }

        public async void LoadData()
        {
            await App.Current.MainPage.Navigation.PushPopupAsync(new LoadingPopup());

            var data = await App.Database.GetItemsAsync();
            if (data != null)
            {
                objModel = new ObservableCollection<CognitiveServiceModel>();
                foreach (var item in data)
                {
                    objModel.Add(JsonConvert.DeserializeObject<CognitiveServiceModel>(item.CognitiveServiceModel));
                }
                SetProperty(ref objModel, objModel);
                OnPropertyChanged("ObjModel");
                await App.Current.MainPage.Navigation.PopPopupAsync();

            }
            else
            {
                await App.Current.MainPage.Navigation.PopPopupAsync();
                await App.Current.MainPage.DisplayAlert("No Data", "No Data availble currently", "ok");

            }


        }
        private void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }

    }
}
