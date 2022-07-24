// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using FindMyBLEDevice.Services.Location;
using CoreLocation;
using Xamarin.Forms;

[assembly: Dependency(typeof(FindMyBLEDevice.iOS.Services.Location))]
namespace FindMyBLEDevice.iOS.Services
{
    public class Location : IPlatformSpecificLocation
    {
        public bool IsLocationServiceEnabled()
        {
            return CLLocationManager.LocationServicesEnabled;
        }
    }
}