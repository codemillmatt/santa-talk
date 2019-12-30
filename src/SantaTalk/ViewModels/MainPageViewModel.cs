using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace SantaTalk
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            SendLetterCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(KidsName, LetterText, _photoStream));
            });

            CameraCommand = new Command(async () => await TakePicture(), () => !IsBusy);
        }

        string kidsName;
        public string KidsName
        {
            get => kidsName;
            set => SetProperty(ref kidsName, value);
        }

        string letterText = "Dear Santa...";
        public string LetterText
        {
            get => letterText;
            set => SetProperty(ref letterText, value);
        }

        private ImageSource _picture;
        public ImageSource Picture
        {
            get => _picture;
            set => SetProperty(ref _picture, value);
        }

        private Stream _photoStream;

        public ICommand SendLetterCommand { get; }
        public ICommand CameraCommand { get; }

        private async Task TakePicture()
        {
            try
            {
                IsBusy = true;

                Picture = null;

                await CrossMedia.Current.Initialize();

                if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
                {
                    MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = $"santa-{DateTime.Now.ToString("ddMMyyyyTHHmmss")}.jpg",
                        MaxWidthHeight = 1000,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        SaveToAlbum = true
                    });

                    if (photo != null)
                    {
                        _photoStream = photo.GetStream();
                        Picture = ImageSource.FromStream(() =>
                        {
                            var stream = photo.GetStream();
                            photo.Dispose();
                            return stream;
                        });
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Camera unavailable.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
