// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Geolocation
{
    public interface IGeolocation
    {
        void CancelLocationSearch();
        Task<Xamarin.Essentials.Location> GetCurrentLocation();
    }
}