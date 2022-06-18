// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel()
        {
            Title = "MapSearch";
        }

        private Location currentLocation;
        public Location CurrentLocation { 
            get { return currentLocation; } 
            set { 
                SetProperty(ref currentLocation, value);
            } 
        }

        public async void OnAppearing()
        {
            CurrentLocation = await App.Geolocation.GetCurrentLocation();
        }

    }
}