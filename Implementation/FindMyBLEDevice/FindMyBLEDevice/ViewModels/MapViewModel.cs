// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using FindMyBLEDevice.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using FindMyBLEDevice.Views;

namespace FindMyBLEDevice.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        private string _selectedDeviceString;
        public MapViewModel(Xamarin.Forms.Maps.Map map)
        {
            Title = "MapSearch";
            this.map = map;
            _selectedDeviceString = "Please selecet a device first.";
            OpenInfoPageCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(InfoPage)}"));
            SelectDevice = new Command(async () => await Shell.Current.GoToAsync($"{nameof(ItemsPage)}"));
        }

        public BTDevice Device { get => App.DevicesStore.SelectedDevice;  }

        private readonly Xamarin.Forms.Maps.Map map;

        public string SelectedDeviceString
        {
            get => _selectedDeviceString;
            set => SetProperty(ref _selectedDeviceString, value);
        }
        public Command OpenInfoPageCommand { get; }
        public Command SelectDevice { get; }



        public async void OnAppearing()
        {
            if (!(App.DevicesStore.SelectedDevice is null))
            {
                SelectedDeviceString = "" + App.DevicesStore.SelectedDevice.UserLabel + "\nClick to select another device.";
            }

                //updates device label above map when opened via the flyout menu
                OnPropertyChanged(nameof(Device));

            var currentLocation = await App.Geolocation.GetCurrentLocation();
            if (currentLocation == null)
            {
                Console.WriteLine("No Location found!");
            } else {
                map.IsShowingUser = true;
                var phonePosition = new Position(currentLocation.Latitude, currentLocation.Longitude);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    phonePosition,
                    Device is null
                        ? Distance.FromKilometers(1)
                        : Distance.BetweenPositions(
                            phonePosition,
                            new Position(Device.LastGPSLatitude, Device.LastGPSLongitude))
                ));
            }
            showSelectedDevice();
        }

        private void showSelectedDevice()
        {
			if (Device is null) return;

            var deviceLocation = new Location(Device.LastGPSLatitude, Device.LastGPSLongitude);
            Pin devicePin = new Pin
            {
                Label = Device.UserLabel,
                Address = Device.LastGPSTimestamp.ToString(),
                Type = PinType.Place,
                Position = new Position(deviceLocation.Latitude, Device.LastGPSLongitude)
            };
            map.Pins.Add(devicePin);
        } 

        public void OnDisappearing() {
            // comment to make linter happy, method will be used in the future
        }
    }
}
