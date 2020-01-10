﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.StateSquid;

namespace SantaTalk
{
    public partial class ResultsPage : ContentPage
    {
        private readonly int _formsWidth;
        private readonly int _formsHeight;

        private bool _initialized = false;
        private bool _starsAdded = false;
        private List<VisualElement> _stars = new List<VisualElement>();
        private ResultsPageViewModel vm = new ResultsPageViewModel();

        public ResultsPage(string kidsName, string letterText)
        {
            InitializeComponent();

            BindingContext = vm;

            vm.KidsName = kidsName;
            vm.LetterText = letterText;
            vm.CurrentState = State.Loading;

            _formsWidth = Convert.ToInt32(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density);
            _formsHeight = Convert.ToInt32(DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
            vm.preparePictureSantaAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.SendLetterToSanta();

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
    }
}
