﻿// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Android.Content;
using Android.Locations;

using System;
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