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

namespace FindMyBLEDevice.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public bool DeviceNotNull => Device != null;
        public Command OpenMapPin { get; }
        public MapViewModel(Xamarin.Forms.Maps.Map map)
        {
            Title = "MapSearch";
            OpenMapPin = new Command( async () => await OpenMapswithPin());
            this.map = map;
        }

        async Task OpenMapswithPin()
        {
            await Xamarin.Essentials.Map.OpenAsync(Device.LastGPSLatitude, Device.LastGPSLongitude, new MapLaunchOptions { Name = Device.UserLabel });
        }

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
            ShowSelectedDevice();
        }

        private void ShowSelectedDevice()
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
