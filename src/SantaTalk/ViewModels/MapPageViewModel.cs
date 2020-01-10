using Acr.UserDialogs;
using MvvmHelpers;
using SantaTalk.Models;
using SantaTalk.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SantaTalk.ViewModels
{
    public class MapPageViewModel : BaseViewModel
    {
        private ObservableCollection<MapModel> _locations;
        public ObservableCollection<MapModel> Locations
        {
            get => _locations;
            set => SetProperty(ref _locations, value);
        }

        public MapPageViewModel()
        {

        }

        public async Task LoadData()
        {
            using (UserDialogs.Instance.Loading("Loading.."))
            {
                List<PinModel> positions = await MobileServiceClientService.Instance.GetPinModelAsync();
                if (positions == null)
                {
                    await UserDialogs.Instance.AlertAsync("No Data availble currently");
                    return;
                }

                Locations = new ObservableCollection<MapModel>();
                foreach (PinModel pin in positions)
                {
                    Locations.Add(new MapModel
                    {
                        Position = new Xamarin.Forms.Maps.Position(pin.Lat, pin.Log),
                        Label = pin.Label,
                        Address = pin.Address,
                        Type = (Xamarin.Forms.Maps.PinType)pin.Type,
                    });
                }
            }
        }
    }
}
