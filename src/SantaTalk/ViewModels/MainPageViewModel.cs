using System;
using System.Windows.Input;
using MvvmHelpers;
using Plugin.Media;
using Xamarin.Forms;

namespace SantaTalk
{
    public class MainPageViewModel : BaseViewModel
    {
        public string FilePath { get; set; }

        ImageSource photo;
        public ImageSource Photo
        {
            get => photo;
            set => SetProperty(ref photo, value);
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

        bool readyToSend;
        public bool ReadyToSend
        {
            get => readyToSend;
            set => SetProperty(ref readyToSend, value);
        }

        public ICommand SendLetterCommand { get; }

        public ICommand TakePictureCommand { get; }

        public ICommand SendPhotoCommand { get; set; }

        public MainPageViewModel()
        {
            SendLetterCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(KidsName, LetterText, FilePath));
            });

            TakePictureCommand = new Command(async () =>
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    Console.Write(@"No Camera :( No camera available");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;

                FilePath = file.Path;

                Photo = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            });
        }
    }
}
