// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using Xamarin.Forms;
using FindMyBLEDevice.Models;
using System;
using FindMyBLEDevice.Views;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using FindMyBLEDevice.Services.Settings;

namespace FindMyBLEDevice.ViewModels
{
    public class StrengthViewModel : BaseViewModel
    {
        private readonly double meterScaleMin;
        private readonly double meterScaleMax;
        private readonly List<int> rssiBuff;
        
        private int _radius;
        private double _meter;
        private int _currentRssi;

        private string _status;

        public StrengthViewModel()
        {
            Title = "StrengthSearch";
            meterScaleMin = rssiToMeter(0);
            meterScaleMax = rssiToMeter(-100);
            rssiBuff = new List<int>();
            _status = "Uninitialized";
        }

        public BTDevice Device
        {
            get => App.DevicesStore.SelectedDevice;
        }

        public int Radius
        {
            get => _radius;
            set => SetProperty(ref _radius, value);
        }

        public double Meter
        {
            get => _meter;
            set
            {
                SetProperty(ref _meter, value);
                Radius = otherScaleToRadius(meterScaleMin, meterScaleMax, Meter);
            }
        }

        public int CurrentRssi
        {
            get => _currentRssi;
            set
            {
                SetProperty(ref _currentRssi, value);
                Meter = rssiToMeter(CurrentRssi);
            }
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public void OnAppearing()
        {
            if (App.DevicesStore.SelectedDevice is null)
            {
                Status = "No device selected!\nPlease select a device to continue.";
            }
            else
            {
                Status = "Connecting to \"" + App.DevicesStore.SelectedDevice.UserLabel + "\"...\n" +
                    "If this takes longer than a few seconds, the device is probably out of range or turned off.";
                App.Bluetooth.StartRssiPolling(App.DevicesStore.SelectedDevice.BT_GUID, (int v) =>
                {
                    int rssiInterval = Preferences.Get(SettingsNames.RssiInterval, Constants.RssiIntervalDefault);
                    int buffSize = rssiInterval > 0 
                        ? Math.Min(Constants.RssiBufferDuration / rssiInterval, Constants.RssiBufferMaxSize)
                        : Constants.RssiBufferMaxSize;
                    rssiBuff.Add(v);
                    while (rssiBuff.Count > buffSize) rssiBuff.RemoveAt(0);
                    CurrentRssi = (int)rssiBuff.Average();

                    if(Meter <= Constants.MeterClosebyThreshold)
                    {
                        Status = "\"" + App.DevicesStore.SelectedDevice.UserLabel + "\" is very close!\n"+
                            "Try searching the vicinity to find it.";
                    }
                    else
                    {
                        Status = "Connected to \"" + App.DevicesStore.SelectedDevice.UserLabel + "\".\n" +
                            "Move around to see in which direction the signal gets better.";
                    }
                }, () =>
                {
                    Status = "Connected to \"" + App.DevicesStore.SelectedDevice.UserLabel + "\".\n" +
                        "Move around to see in which direction the signal gets better.";
                }, () =>
                {
                    Status = "Disconnected! Trying to reconnect...\n" +
                        "Please move back into range of the device.";
                    CurrentRssi = -100;
                    Meter = meterScaleMax;
                });
            }
        }

        public void OnDisappearing()
        {
            App.Bluetooth.StopRssiPolling();
        }
        
        /// <summary>
        /// Calculates the strengts views radius from any given input value and its scale.
        /// </summary>
        /// <param name="scaleMin">Min value of the input value's scale.</param>
        /// <param name="scaleMax">Max value of the input value's scale.</param>
        /// <param name="value">Input value</param>
        /// <returns>circle radius</returns>
        private int otherScaleToRadius(double scaleMin, double scaleMax, double value)
        {
            const int radiusMin = 30;
            const int radiusMax = 400;
            const int radiusScaleSize = radiusMax - radiusMin;
            double inputScaleSize = scaleMax - scaleMin;

            double relScalePosition = (value - scaleMin) / inputScaleSize;
            int resultingRadius = radiusMin + (int)(relScalePosition * radiusScaleSize);
            return resultingRadius;
        }

        private double rssiToMeter(int rssi)
        {
            // https://medium.com/beingcoders/convert-rssi-value-of-the-ble-bluetooth-low-energy-beacons-to-meters-63259f307283 
            // TODO replace the constants with polled values
            const int measuredPower = -60;
            const int environmentalFactor = 3;
            double dist = Math.Pow(10, (double)(measuredPower - rssi) / (10 * environmentalFactor));
            return dist;
        }
    }
}