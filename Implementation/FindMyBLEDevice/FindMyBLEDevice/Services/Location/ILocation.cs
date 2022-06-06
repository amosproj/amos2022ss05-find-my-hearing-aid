// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@fau.de>

using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FindMyBLEDevice.Services.Location
{
    public interface ILocation
    {
        Task<PermissionStatus> CheckAndRequestLocationPermission();
    }
}