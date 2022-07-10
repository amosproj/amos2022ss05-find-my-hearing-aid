// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using FindMyBLEDevice.Services.Geolocation;
using FindMyBLEDevice.Services.Database;

namespace FindMyBLEDevice.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        private readonly Xamarin.Forms.Maps.Map map;
        private readonly IGeolocation geolocation;
        private readonly IDevicesStore devicesStore;

        public Command OpenMapPin { get; }

        public BTDevice Device => devicesStore.SelectedDevice;
        public bool DeviceNotNull => Device != null;

        public MapViewModel(Xamarin.Forms.Maps.Map map, IGeolocation geolocation, IDevicesStore devices)
        {
            Title = "MapSearch";

            this.map = map;
            this.geolocation = geolocation;
            this.devicesStore = devices;

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
            ShowSelectedDevice();
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
    }
}
