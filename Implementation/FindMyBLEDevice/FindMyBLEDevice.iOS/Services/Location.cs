// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

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