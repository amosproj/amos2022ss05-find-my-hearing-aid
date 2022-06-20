// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FindMyBLEDevice.Services.Geolocation
{
    public class GeolocationAccess : IGeolocationAccess
    {
        public Task<Xamarin.Essentials.Location> GetLocationAsync(GeolocationRequest request, CancellationToken token)
        {
            return Xamarin.Essentials.Geolocation.GetLocationAsync(request, token);
        }
    }
}
