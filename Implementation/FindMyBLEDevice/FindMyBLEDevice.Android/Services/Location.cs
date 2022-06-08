using Android.App;
using Android.Content;
using Android.OS;
// X - License - Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FindMyBLEDevice.Services.Location;
using Xamarin.Forms;

[assembly: Dependency(typeof(FindMyBLEDevice.Droid.Services.Location))]
namespace FindMyBLEDevice.Droid.Services
{
    internal class Location : IPlatformSpecificLocation
    {
        public bool IsLocationServiceEnabled()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);

            try
            {
                return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}