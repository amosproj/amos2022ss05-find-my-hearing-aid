// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Settings;
using FindMyBLEDevice.Services.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public class RssiBuffer
    {
        protected double rssi = 0;

        public virtual double Get()
        {
            return this.rssi;
        }

        public virtual void Update(double newRssi)
        {
            this.rssi = newRssi;
        }
    }

    public class RssiRingBuffer : RssiBuffer 
    {
        protected readonly ISettings settings;
        protected readonly List<double> rssiBuff;

        public RssiRingBuffer(ISettings settings)
        {
            this.settings = settings;
            rssiBuff = new List<double>();
        }

        public override void Update(double newRssi)
        {
            rssiBuff.Add(newRssi);
            int rssiInterval = settings.Get(SettingsNames.RssiInterval, Constants.RssiIntervalDefault);
            int buffSize = rssiInterval > 0
                ? Math.Min(Constants.RssiBufferDuration / rssiInterval, Constants.RssiBufferMaxSize)
                : Constants.RssiBufferMaxSize;
            while (rssiBuff.Count > buffSize) rssiBuff.RemoveAt(0);
            base.Update(rssiBuff.Average());
        }
    }

    public class RssiExponentialBuffer : RssiBuffer
    {
        // meters per degree of lat/lon = 2 * PI * radius of earth / 360 degrees
        protected const double metersPerDegree = 2 * Math.PI * 6371000 / 360;
        // history-factor for exponential moving average
        protected const double base_alpha = 0.95;
        // how much of the non-history factor (1 - alpha) is dependent on GPS movement
        protected const double beta = 0.75;
        protected const double base_movement = 10;

        protected readonly IGeolocation geolocation;

        protected volatile Xamarin.Essentials.Location previousLocation, currentLocation;
        protected volatile Boolean fetchingLocation;

        public RssiExponentialBuffer(IGeolocation geolocation)
        {
            this.geolocation = geolocation;
            fetchingLocation = false;
        }

        public override void Update(double newRssi)
        {
            if(previousLocation == null)
            {
                //initialize
                this.rssi = newRssi;
                var res = geolocation.GetCurrentLocation();
                res.Wait();
                previousLocation = currentLocation = res.Result;
                return;
            } 
            else if (!fetchingLocation)
            {
                fetchingLocation = true;
                Task.Run(async () =>
                {
                    try
                    {
                        var fetchedLocation = await geolocation.GetCurrentLocation();
                        previousLocation = currentLocation;
                        currentLocation = fetchedLocation;
                    }
                    finally
                    {
                        fetchingLocation = false;
                    }
                });
            }
            double dLat = currentLocation.Latitude - previousLocation.Latitude, dLon = currentLocation.Longitude - previousLocation.Longitude;
            // this does currently not account for spherical distortion (the farther away from the equator you are,
            // the less distance a change in longitude means), but it will do for now
            double variation = Math.Sqrt(dLat * dLat + dLon * dLon);

            double movement_factor = Math.Exp(-variation * metersPerDegree / base_movement);

            double alpha = base_alpha + (1 - base_alpha) * beta * movement_factor;

            // simple exponential moving average
            base.Update(alpha * rssi + (1 - alpha) * newRssi);
        }
    }
}
