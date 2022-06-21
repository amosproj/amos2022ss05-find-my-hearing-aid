// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using FindMyBLEDevice.Models;
using System;

namespace FindMyBLEDevice.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel(Xamarin.Forms.Maps.Map map)
        {
            Title = "MapSearch";
            this.map = map;
        }

        private Xamarin.Forms.Maps.Map map;
        private Location currentLocation;
        public Location CurrentLocation { 
            get { return currentLocation; } 
            set { 
                SetProperty(ref currentLocation, value);
            } 
        }


        public BTDevice Device
        {
            get => App.DevicesStore.SelectedDevice;
        }

        public async void OnAppearing()
        {
            CurrentLocation = await App.Geolocation.GetCurrentLocation();
            if (CurrentLocation == null)
            {
                Console.WriteLine("No Location found!");
            } else {
                Pin pin = new Pin
                {
                    Label = "Your Smartphone",
                    Address = "",
                    Type = PinType.Place,
                    Position = new Position(CurrentLocation.Latitude, CurrentLocation.Longitude)
                };
                map.Pins.Add(pin);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromKilometers(1)));
            }
        }

        public void OnDisappearing() {
            // comment to make linter happy, method will be used in the future
        }
    }
}