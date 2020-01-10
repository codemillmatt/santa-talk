using System.Windows.Input;
using System.Threading.Tasks;
using MvvmHelpers;
using Xamarin.Forms;

namespace SantaTalk
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            SendLetterCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(KidsName, LetterText));
            });

            ScanLetterCommand = new Command<bool>(async (useCamera) =>
            {
                await ScanLetterForSanta(useCamera);
            });
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

        public ICommand SendLetterCommand { get; }
        public ICommand ScanLetterCommand { get; }

        private async Task ScanLetterForSanta(bool useCamera)
        {
            var photoService = new PhotoService();

            var photo = useCamera ? await photoService.TakePhoto() : await photoService.ChoosePhoto();

            var scanService = new LetterScanService();
            var scannedLetter = await scanService.ScanLetterForSanta(photo.GetStream());

            LetterText = scannedLetter;
        }
    }
}