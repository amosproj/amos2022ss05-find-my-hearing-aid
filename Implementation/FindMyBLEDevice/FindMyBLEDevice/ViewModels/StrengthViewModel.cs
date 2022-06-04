// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Xamarin.Forms;
using FindMyBLEDevice.Models;
using System;

namespace FindMyBLEDevice.ViewModels
{
    [QueryProperty(nameof(DeviceId), nameof(DeviceId))]
    public class StrengthViewModel : BaseViewModel
    {
        private readonly double meterScaleMin;
        private readonly double meterScaleMax;
                
        private BTDevice _device;
        private int _radius;
        private int _radiusDrag;
        private double _meter;
        private int _currentRssi;
        private int _deviceId;

        public StrengthViewModel()
        {
            Title = "StrengthSearch";
            meterScaleMin = rssiToMeter(0);
            meterScaleMax = rssiToMeter(-100);
        }

        public int RssiPollInterval
        {
            get => App.Bluetooth.RssiPollInterval;
        }

        public BTDevice Device
        {
            get => _device;
            set => SetProperty(ref _device, value);
        }

        public int DeviceId
        {
            get => _deviceId;
            set => SetProperty(ref _deviceId, value);
        }

        public int Radius
        {
            get => _radius;
            set
            {
                _radiusDrag = Radius;
                SetProperty(ref _radius, value);
            }
        }

        public int RadiusDrag
        {
            get => _radiusDrag;
            set => SetProperty(ref _radiusDrag, value);
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

        public async void OnAppearing()
        {

            Device = await App.DevicesStore.GetDevice(_deviceId);
            await App.Bluetooth.StartRssiPolling(Device.BT_GUID, (int v) => {
                CurrentRssi = v;
                return 0;
            });

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

            double relScalePosition = (double)(value - scaleMin) / inputScaleSize;
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