// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IDevicesStore devicesStore;
        private readonly string _message = "Welcome to our 'Find your BLE devices'-App.\n"
                                + "By clicking on the question mark, you will receive individual help on the different pages of our app.\n"
                                + "Our two core functionalities consist of 'Strength Search' and 'Map Search':\n"
                                + "- 'Strength Search' measures the distance to your lost device based on the emitting Bluetooth signal.\n"
                                + "- 'Map Search' shows the last known GPS coordinate of your lost device.\n"
                                + "- The tab bar at the bottom of this page allows you to directly navigate to your desired function.\n"
                                + "- With the button above the tab bar, you can select your desired device.";

        public BTDevice Device => devicesStore.SelectedDevice;

        private string _selectedDeviceString;
        public string SelectedDeviceString
        {
            get => _selectedDeviceString;
            set => SetProperty(ref _selectedDeviceString, value);
        }

        public Command OpenMapPageCommand { get; }
        public Command OpenStrengthPageCommand { get; }
        public Command ShowInfoPage { get; }
        public Command SelectDevice { get; }

        public AboutViewModel(INavigator navigator, IDevicesStore devicesStore)
        {
            Title = "Home";

            this.devicesStore = devicesStore;

            OpenMapPageCommand = new Command(
                async () => await navigator.GoToAsync(navigator.MapPage, true));
            OpenStrengthPageCommand = new Command(
                async () => await navigator.GoToAsync(navigator.StrengthPage, true));
            SelectDevice = new Command(
                async () => await navigator.GoToAsync(navigator.DevicesPage));
            ShowInfoPage = new Command(
                async () => await App.Current.MainPage.DisplayAlert("Information", _message, "Ok"));
        }

        public void OnAppearing()
        {
            if (!(devicesStore.SelectedDevice is null))
            {
                SelectedDeviceString = "" + devicesStore.SelectedDevice.UserLabel + "\n> Click to select a different device <";
            } else
            {
                SelectedDeviceString = "No device selected!\n> Click to select a device <";
            }
        }
    }
}
