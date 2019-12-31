using MvvmHelpers;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SantaTalk.Models;
using SantaTalk.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Maps;

namespace SantaTalk.ViewModels
{
    public class UsersMapPageViewModel : BaseViewModel
    {
        public ObservableCollection<MapModel> Locations
        {
            get
            {
                return _locations;
            }
            set
            {

                if (_locations != value)
                {
                    _locations = value;
                }
            }
        }
        private ObservableCollection<MapModel> _locations;
        public UsersMapPageViewModel()
        {

        }

        public async void LoadData()
        {
            await App.Current.MainPage.Navigation.PushPopupAsync(new LoadingPopup());

            var data = await App.Database.GetItemsAsync();
            if (data != null)
            {
                var objModel = new ObservableCollection<CognitiveServiceModel>();
                foreach (var item in data)
                {
                    objModel.Add(JsonConvert.DeserializeObject<CognitiveServiceModel>(item.CognitiveServiceModel));
                }
                _locations = new ObservableCollection<MapModel>();
                foreach (var item in objModel)
                {
                    
                    _locations.Add(new MapModel {position=new Position(item.Latitude,item.Longitude),Expression=item.Score,Name=item.Name });
                }
                SetProperty(ref _locations, _locations);
                OnPropertyChanged("Locations");
                await App.Current.MainPage.Navigation.PopPopupAsync();

            }
            else
            {
                await App.Current.MainPage.Navigation.PopPopupAsync();
                await App.Current.MainPage.DisplayAlert("No Data", "No Data availble currently", "ok");

            }
        }
    }
}
