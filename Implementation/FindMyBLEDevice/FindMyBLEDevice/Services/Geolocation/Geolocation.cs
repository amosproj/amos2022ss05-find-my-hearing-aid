// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FindMyBLEDevice.Services.Geolocation
{
    public class Geolocation : IGeolocation
    {
        private CancellationTokenSource cts;
        private readonly IGeolocationAccess _geolocationAccess;

        public Geolocation() : this(new GeolocationAccess()) { }
        public Geolocation(IGeolocationAccess geolocationAccess)
        {
            _geolocationAccess = geolocationAccess;
        }

        public async Task<Xamarin.Essentials.Location> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await _geolocationAccess.GetLocationAsync(request, cts.Token);

                Console.WriteLine($"Latitude: {location?.Latitude}, Longitude: {location?.Longitude}, Altitude: {location?.Altitude}");
                return location;
            }
            catch (PermissionException)
            {
                Console.WriteLine("No permission");
                // Handle permission exception
            }
            catch (Exception)
            {
                // Unable to get location
            }
            return null;
        }

        public void CancelLocationSearch()
        {
            if (cts != null && !cts.IsCancellationRequested) cts.Cancel();
        }
    }
}
