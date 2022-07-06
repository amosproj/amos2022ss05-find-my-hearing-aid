// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using FindMyBLEDevice.Models;
using System;
using FindMyBLEDevice.Services.ForegroundService;
using Xamarin.Forms;
using System.Linq;

namespace FindMyBLEDevice.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        private readonly INavigator navigator;

        private bool userDeclinedPageSwitch;
        private bool showingDialogue;

        public MapViewModel(Xamarin.Forms.Maps.Map map, INavigator navigator)
        {
            Title = "MapSearch";
            this.map = map;
            this.navigator = navigator;
        }

        public MapViewModel(Xamarin.Forms.Maps.Map map) : this(map, App.Navigator) { }

        public BTDevice Device { get => App.DevicesStore.SelectedDevice;  }

        private readonly Xamarin.Forms.Maps.Map map;

        public async void OnAppearing()
        {
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

            userDeclinedPageSwitch = false;
            showingDialogue = false;
            App.ForegroundService.ServiceIteration += CheckIfSelectedDeviceReachable;
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
            App.ForegroundService.ServiceIteration -= CheckIfSelectedDeviceReachable;
        }

        private async void CheckIfSelectedDeviceReachable(object sender, ForegroundServiceEventArgs ea)
        {
            if (showingDialogue || userDeclinedPageSwitch || Device is null) return;
            showingDialogue = true;

            var device = ea.Devices.Where(d => d.ID == Device.ID);
            if (!device.Any()) return;

            if (device.First().WithinRange)
            {
                bool promptAnswer = false;
                await Xamarin.Forms.Device.InvokeOnMainThreadAsync(async () => {
                    promptAnswer = await Application.Current.MainPage.DisplayAlert($"BLE Signal From {Device.UserLabel} Detected", $"Do you want to switch to the signal strength search?", "Yes", "No");
                });
                if (promptAnswer)
                {
                    await Xamarin.Forms.Device.InvokeOnMainThreadAsync(async () => {
                        await navigator.GoToAsync(navigator.StrengthPage, true);
                    });
                }
                else
                {
                    userDeclinedPageSwitch = true;
                }
            }

            showingDialogue = false;
        }
    }
}
