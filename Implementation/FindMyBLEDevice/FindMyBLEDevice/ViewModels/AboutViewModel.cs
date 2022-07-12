// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public Command OpenMapPageCommand { get; }
        public Command OpenStrengthPageCommand { get; }

        public AboutViewModel(INavigator navigator)
        {
            Title = "About";
            
            OpenMapPageCommand = new Command(
                async () => await navigator.GoToAsync(navigator.MapPage));
            OpenStrengthPageCommand = new Command(
                async () => await navigator.GoToAsync(navigator.StrengthPage));
        }
    }
}
