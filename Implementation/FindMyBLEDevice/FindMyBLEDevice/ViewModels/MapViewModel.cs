﻿// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services;
using Xamarin.Forms;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Geolocation;
using System.Threading.Tasks;
using System;

namespace FindMyBLEDevice.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public bool DeviceNotNull => Device != null;

        private readonly Xamarin.Forms.Maps.Map map;
        private readonly IGeolocation geolocation;
        private readonly INavigator navigator;
        private readonly IDevicesStore devicesStore;
        private bool userDeclinedPageSwitch, showingDialogue;

        public BTDevice Device { get => devicesStore.SelectedDevice;  }

        public Command OpenMapPin { get; }
        public MapViewModel(Xamarin.Forms.Maps.Map map, IGeolocation geolocation, INavigator navigator, IDevicesStore devicesStore)
        {
            Title = "MapSearch";
            OpenMapPin = new Command(async () => await OpenMapswithPin());
            this.map = map;
            this.geolocation = geolocation;
            this.navigator = navigator;
            this.devicesStore = devicesStore;

            OpenMapPin = new Command(
                async () => await OpenMapswithPin());

            PropertyChanged += DeviceChanged;
        }

        private void DeviceChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Device))
                OnPropertyChanged(nameof(DeviceNotNull));
        }

        private async Task OpenMapswithPin()
        {
            await Xamarin.Essentials.Map.OpenAsync(Device.LastGPSLatitude, Device.LastGPSLongitude,
                new MapLaunchOptions { Name = Device.UserLabel });
        }

        private void ShowSelectedDevice()
        {
            if (Device is null) return;

            map.Pins.Clear();
            Pin devicePin = new Pin
            {
                Label = Device.UserLabel,
                Address = Device.LastGPSTimestamp.ToString(),
                Type = PinType.Place,
                Position = new Position(Device.LastGPSLatitude, Device.LastGPSLongitude)
            };
            map.Pins.Add(devicePin);
        }

        private async void CheckIfSelectedDeviceReachable(object sender, EventArgs ea)
        {
            if (showingDialogue || Device is null) return;
            showingDialogue = true;

            BTDevice updatedDevice = null;
            try
            {
                updatedDevice = await devicesStore.GetDevice(Device.ID);
            } catch (ArgumentException ae)
            {
                Console.WriteLine($"[MapPage] Error retrieving selected device from database");
            }
            if (updatedDevice is null) return;

            if (updatedDevice.WithinRange)
            {
                devicesStore.DevicesChanged -= CheckIfSelectedDeviceReachable;
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
            }

            showingDialogue = false;
        }

        public async void OnAppearing()
        {
            //updates device label above map when opened
            OnPropertyChanged(nameof(Device));

            await CheckBluetoothAndLocation.Check();

            var currentLocation = await geolocation.GetCurrentLocation();
            if (currentLocation != null)
            {
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

            userDeclinedPageSwitch = false;
            showingDialogue = false;
            devicesStore.DevicesChanged += CheckIfSelectedDeviceReachable;

            ShowSelectedDevice();
        }

        public void OnDisappearing() {
            devicesStore.DevicesChanged -= CheckIfSelectedDeviceReachable;
        }
    }
}
