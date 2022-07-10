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
        private IGeolocationAccess _geolocationAccess;

        public Geolocation() : this(new GeolocationAccess()) { }
        public Geolocation(IGeolocationAccess geolocationAccess)
        {
            _geolocationAccess = geolocationAccess;
        }

        public async Task<Xamarin.Essentials.Location> GetCurrentLocation()
        {
#pragma warning disable CS0168 // Variable ist deklariert, wird jedoch niemals verwendet
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await _geolocationAccess.GetLocationAsync(request, cts.Token);

                Console.WriteLine($"Latitude: {location?.Latitude}, Longitude: {location?.Longitude}, Altitude: {location?.Altitude}");
                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine("No permission");
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
#pragma warning restore CS0168 // Variable ist deklariert, wird jedoch niemals verwendet
            return null;
        }

        public async Task CancelLocationSearch()
        {
            if (cts != null && !cts.IsCancellationRequested) cts.Cancel();
        }
    }
}
