using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace SantaTalk
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        ObservableCollection<MediaFile> files = new ObservableCollection<MediaFile>();

        public MainPageViewModel()
        {
            IsPictureTaken = false;

            SendLetterCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(KidsName, LetterText, PictureBase64, Picture));
            });

            //TakePhoto = new Command(async () =>
            //{
            //    await CrossMedia.Current.Initialize();

            //    files.Clear();

            //    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            //    {
            //        await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
            //        return;
            //    }

            //    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            //    {
            //        PhotoSize = PhotoSize.Medium,
            //        Directory = "SantaTalk",
            //        Name = "pic.jpg"
            //    });

            //    if (file == null)
            //        return;

            //    Picture = file.Path;
            //    IsPictureTaken = true;

            //    files.Add(file);

            //    // Convert to Base64
            //    var memoryStream = new MemoryStream();
            //    file.GetStream().CopyTo(memoryStream);
            //    PictureBase64 = Convert.ToBase64String(memoryStream.ToArray());
            //});

            TakePhoto = new Command(async () =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium

                });

                if (file == null)
                    return;

                Picture = file.Path;
                IsPictureTaken = true;

                // Convert to Base64
                var memoryStream = new MemoryStream();
                file.GetStream().CopyTo(memoryStream);
                PictureBase64 = Convert.ToBase64String(memoryStream.ToArray());

            });
        }

        string kidsName;
        public string KidsName { get => kidsName; set => kidsName = value; }

        string letterText = "Dear Santa...";
        public string LetterText { get => letterText; set => letterText = value; }

        string picture;
        public string Picture
        {
            get => picture;
            set
            {
                picture = value;
                OnPropertyChanged("Picture");
            }
        }

        public string PictureBase64 { get; set; }

        bool isPictureTaken;
        public bool IsPictureTaken
        {
            get => isPictureTaken;
            set
            {
                isPictureTaken = value;
                OnPropertyChanged("IsPictureTaken");
            }
        }

        public ICommand SendLetterCommand { get; }

        public ICommand TakePhoto { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
