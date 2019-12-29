using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SantaTalk
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly int _formsWidth;
        private readonly int _formsHeight;

        private bool _initialized = false;
        private bool _starsAdded = false;
        private List<VisualElement> _stars = new List<VisualElement>();

        public MainPage()
        {
            InitializeComponent();

            _formsWidth = Convert.ToInt32(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density);
            _formsHeight = Convert.ToInt32(DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
        }

        private const int _leftEyeDefaultPosition = -29;
        private const int _rightEyeDefaultPosition = 29;

        private void PositionEyes()
        {
            LeftEye.TranslationX = _leftEyeDefaultPosition;
            RightEye.TranslationX = _rightEyeDefaultPosition;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            PositionEyes();

            if (!_initialized)
            {
                PositionStars();
                RotateStars();
            }

            _initialized = true;
        }

        private void PositionStars()
        {
            if (!_starsAdded)
            {
                var random = new Random();


                for (int j = 0; j < 5; j++)
                {
                    var starField = new Grid();

                    for (int i = 0; i < 20; i++)
                    {
                        var size = random.Next(3, 7);
                        var star = new CachedImage() { Source = "star.png", Opacity = 0.3, HeightRequest = size, WidthRequest = size, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TranslationX = random.Next(0, _formsWidth), TranslationY = random.Next(0, _formsHeight) };
                        starField.Children.Add(star);
                    }

                    _stars.Add(starField);

                    MainGrid.Children.Insert(0, starField);
                }
            }
        }

        private async Task RotateStars()
        {
            var rotateTasks = new List<Task>();
            var random = new Random();

            foreach (var star in _stars)
            {
                var rate = random.Next(240000, 300000);
                rotateTasks.Add(RotateElement(star, (uint)rate, new CancellationToken()));
            }

            await Task.WhenAll(rotateTasks);
        }

        async Task RotateElement(VisualElement element, uint duration, CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                await element.RotateTo(360, duration, Easing.Linear);
                await element.RotateTo(0, 0); // reset to initial position
            }
        }

        private void OnEntryFocused(object sender, FocusEventArgs args)
        {
            if (sender is Entry entry)
            {
                MoveEyes(entry.CursorPosition);
            }
        }

        private void OnEntryUnfocused(object sender, FocusEventArgs args)
        {
            LeftEye.TranslationX = _leftEyeDefaultPosition;
            LeftEye.TranslationY = 0;
            RightEye.TranslationX = _rightEyeDefaultPosition;
            RightEye.TranslationY = 0;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (sender is Entry entry)
            {
                MoveEyes(entry.CursorPosition);
            }
        }

        private const int _minEntryCursorPosition = 0;
        private const int _maxEntryCursorPosition = 40;
        private const int _minShift = -8;
        private const int _maxShift = 8;

        private void MoveEyes(int cursorPosition)
        {
            LeftEye.TranslationX = _leftEyeDefaultPosition + CalculateShift(cursorPosition);
            LeftEye.TranslationY = 3;
            RightEye.TranslationX = _rightEyeDefaultPosition + CalculateShift(cursorPosition);
            RightEye.TranslationY = 3;
        }

        private int CalculateShift(int cursorPosition)
        {
            return Map(cursorPosition, _minEntryCursorPosition, _maxEntryCursorPosition, _minShift, _maxShift);
        }

        private int Map(int value, int fromMin, int fromMax, int toMin, int toMax)
        {
            if (value > _maxEntryCursorPosition)
                value = _maxEntryCursorPosition;

            return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
        }

        private void OnEditorFocused(object sender, FocusEventArgs args)
        {
            LeftEyeSmiling.IsVisible = true;
            RightEyeSmiling.IsVisible = true;
        }

        private void OnEditorUnfocused(object sender, FocusEventArgs args)
        {
            LeftEyeSmiling.IsVisible = false;
            RightEyeSmiling.IsVisible = false;
        }
    }
}
