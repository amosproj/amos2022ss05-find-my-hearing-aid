// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IDevicesStore devicesStore;

        public Command OpenMapPageCommand { get; }
        public Command OpenStrengthPageCommand { get; }
        public Command OpenInfoPageCommand { get; }
        public Command SelectDevice { get; }

        public BTDevice Device => devicesStore.SelectedDevice;

        private string _selectedDeviceString;
        public string SelectedDeviceString
        {
            get => _selectedDeviceString;
            set => SetProperty(ref _selectedDeviceString, value);
        }

        public AboutViewModel(INavigator navigator, IDevicesStore devicesStore)
        {
            Title = "Home";

            this.devicesStore = devicesStore;

            SelectedDeviceString = "No device selected!\n> Click here to select a device <";

            OpenMapPageCommand = new Command(
                async () => await navigator.GoToAsync(navigator.MapPage, true));
            OpenStrengthPageCommand = new Command(
                async () => await navigator.GoToAsync(navigator.StrengthPage, true));
            OpenInfoPageCommand = new Command(
                async () => await navigator.GoToAsync(navigator.InfoPage));
            SelectDevice = new Command(
                async () => await navigator.GoToAsync(navigator.DevicesPage));
        }

        public void OnAppearing()
        {
            if (!(devicesStore.SelectedDevice is null))
            {
                SelectedDeviceString = "" + devicesStore.SelectedDevice.UserLabel + "\n> Click to select a different device <";
            }
        }
    }
}
