using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SantaTalk.Helpers;
using Xamarin.Forms;

namespace SantaTalk
{
    public class MainPageViewModel : BaseViewModel
    {
        private Stream PictureToSanta;

        public ICommand SendLetterCommand { get; }

        public ICommand SendPictureCommand { get; }

        public ICommand TakePictureCommand { get; }

        public MainPageViewModel()
        {
            SendLetterCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(KidsName, LetterText));
            });

            SendPictureCommand = new Command(async () =>
            {
                if (PictureToSanta==null)
                {
                    await UserDialogs.Instance.AlertAsync("Oh oh.. You have not attached the image for Santa ..");
                    return;
                }
                await Application.Current.MainPage.Navigation.PushAsync(new ResultsPageFace(KidsName, LetterText, PictureToSanta));
            });

            TakePictureCommand = new Command(async () => await SendImage(), () => !IsBusy);
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

        private async Task SendImage()
        {
            await CrossMedia.Current.Initialize();

            MediaFile file;

            try
            {
                if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
                {
                    string source = await UserDialogs.Instance.ActionSheetAsync(
                        "Donde va a tomar la imagen?", "Cancelar", null, null, "Desde Galeria", "Desde Camara");

                    if (source == "Cancelar")
                    {
                        file = null;
                        return;
                    }

                    if (source == "Desde Camara")
                    {
                        file = await CrossMedia.Current.TakePhotoAsync(
                            new StoreCameraMediaOptions
                            {
                                Directory = "Sample",
                                Name = $"SantaTalk-{DateTime.Now.ToString("ddMMyyyyTHHmmss")}.jpg",
                                AllowCropping = true,
                                CompressionQuality = 100,
                            }
                        );
                    }
                    else
                    {
                        file = await CrossMedia.Current.PickPhotoAsync(
                        new PickMediaOptions
                        {
                            PhotoSize = PhotoSize.MaxWidthHeight,
                            MaxWidthHeight = 250,
                            CompressionQuality = 85
                        }
                        );
                    }
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync(
                    new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 250,
                        CompressionQuality = 85
                    }
                    );
                }

                if (file == null)
                {
                    return;
                }

                PictureToSanta = file.GetStream();
                Picture = ImageSource.FromStream(() => new MemoryStream(FilesHelper.ReadFully(file.GetStream())));

                file.Dispose();

            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync("Ocurrión un error al cargar la imagen: " + ex.Message, "Error");
            }

        }
    }
}
