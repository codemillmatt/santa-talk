using MvvmHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Rg.Plugins.Popup.Extensions;
using SantaTalk.Helpers;
using SantaTalk.Models;
using SantaTalk.Services;
using SantaTalk.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SantaTalk.ViewModels
{
    class CameraPageViewModel : BaseViewModel
    {
        public ICommand StartCameraCommand { get; set; }
        ComputerVisionAPI objVisionService;
        CognitiveServiceModel objCognitiveServiceModel;
        public CameraPageViewModel()
        {
            InitializeCommands();
            objVisionService = new ComputerVisionAPI();
            Title = "Camera";

        }

        private async void InitializeCommands()
        {
           
            StartCameraCommand = new Command(InitializeCamera);
         

        }

        public void StartCamera()
        {
            Device.BeginInvokeOnMainThread(StartCamera);
        }

        public async void InitializeCamera()
        {
            await CrossMedia.Current.Initialize();

            
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.Navigation.PopPopupAsync();
                await App.Current.MainPage.DisplayAlert("No Camera", "Camera is not available in the device", "Ok");
               
                return;
            }
            var random = new Random();
            int randomNumber =random.Next(1000);
            var randomFileName = "Images" + randomNumber;
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                
                Directory="Santa",
                Name=randomFileName,
                SaveToAlbum=true,
                CompressionQuality=60,
                MaxWidthHeight=50
            });
            if (file==null)
            {
               
                await App.Current.MainPage.DisplayAlert("No Picture Capture", "Picture not captured, try again ", "ok");
               
                return;
            }
            await Application.Current.MainPage.Navigation.PushPopupAsync(new LoadingPopup());
            // CallComputerVision(file.AlbumPath);
            //await MakeAnalysisRequest(file.Path);
            objCognitiveServiceModel = await CallComputerVision(file.Path);

           // string modelContent = await CallComputerVision(file.Path);
           
            if (objCognitiveServiceModel==null)
            {
                await Application.Current.MainPage.Navigation.PopPopupAsync();
                await App.Current.MainPage.DisplayAlert("Issue", "No data returned from vision api", "Ok");
            
                return;
            }

            Dictionary<string,double> locations=await GetLocation();
            if (locations==null)
            {
                await App.Current.MainPage.DisplayAlert("Not found", "No location found", "ok");
                objCognitiveServiceModel.Latitude = 28.7041;
                objCognitiveServiceModel.Longitude = 77.1025;
            }
            else
            {
                double lat,lng;

                locations.TryGetValue("Latitude", out lat);
                locations.TryGetValue("Longitude", out lng);
                objCognitiveServiceModel.Latitude = lat;
                objCognitiveServiceModel.Longitude = lng;
            }
            objCognitiveServiceModel.ImagePath = file.Path;
      
            var result = await App.Database.SaveItemAsync(new VisionModel() { CognitiveServiceModel= JsonConvert.SerializeObject(objCognitiveServiceModel) } );
            if (result!=0)
            {
                await Application.Current.MainPage.Navigation.PopPopupAsync();
                await App.Current.MainPage.DisplayAlert("Successfull", "Record Sucessfully Added in database", "ok");
                return;
                
            }
            else
            {
                await Application.Current.MainPage.Navigation.PopPopupAsync();
                await App.Current.MainPage.DisplayAlert("Failed", "Record not Added in database, try again", "ok");
                return;
            }
            

        }

       

        private async Task<CognitiveServiceModel> CallComputerVision(string pathUrl)
        {
             return await objVisionService.MakeAnalysisRequest(pathUrl);
        }

        public static async Task<Dictionary<string,double>> GetLocation()
        {
            Dictionary<string, double> lstLocation = new System.Collections.Generic.Dictionary<string, double>();
            try
            {

                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    lstLocation.Add("Latitude", location.Latitude);
                    lstLocation.Add("Longitude", location.Longitude);


                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            return lstLocation;
        }
        

    }
}
