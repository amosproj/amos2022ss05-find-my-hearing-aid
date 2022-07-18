// X - License - Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@fau.de>
// SPDX-FileCopyrightText: Jannik Schuetz <jannik.schuetz@gmx.net>

using FindMyBLEDevice.Services.Permission;
using Xamarin.Forms;

[assembly: Dependency(typeof(FindMyBLEDevice.Droid.Services.Permission))]
namespace FindMyBLEDevice.Droid.Services
{
    // These functions only return true, because we only need the iOS implementation for it
    internal class Permission : IPermission
    {
        public bool checkBluetoothPermission()
        {
            return true;
        }

        public bool checkLocationPermission()
        {
            return true;
        }
    }
}