// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@fau.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using UIKit;

using FindMyBLEDevice.Services.Permission;
using CoreLocation;
using Xamarin.Forms;
using CoreBluetooth;

[assembly: Dependency(typeof(FindMyBLEDevice.iOS.Services.Permission))]
namespace FindMyBLEDevice.iOS.Services
{
    public class Permission : IPermission
    {
        public bool CheckBluetoothPermission()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                return CBManager.Authorization == CBManagerAuthorization.AllowedAlways;
            }
            else
            {
                // Bluetooth permission is only needed for iOS version 13 and above, so return true for all other versions
                return true;
            }
        }

        public bool CheckLocationPermission()
        {
            switch (CLLocationManager.Status)
            {
                case CLAuthorizationStatus.Authorized:
                case CLAuthorizationStatus.AuthorizedWhenInUse:
                    return true;
                default:
                    return false;
            }
        }
    }
}