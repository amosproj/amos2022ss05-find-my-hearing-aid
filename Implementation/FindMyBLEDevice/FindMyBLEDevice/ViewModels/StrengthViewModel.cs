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
        public const double MaxRadiusRelativeToScreen = 0.9;
        private const int MinRadiusSize = 30;
        private readonly int MaxRadiusSize;

        private readonly double meterScaleMin;
        private readonly double meterScaleMax;
        private readonly List<int> rssiBuff;

        private List<int> _circleSizes; 
        private int _radius;
        private double _meter;
        private int _currentRssi;
        private int _txPower;

        private string _status;
        private string _selectedDeviceString;


        public StrengthViewModel()
        {
            Title = "StrengthSearch";
            meterScaleMin = rssiToMeter(0, Constants.TxPowerDefault);
            meterScaleMax = rssiToMeter(-100, Constants.TxPowerDefault);
            rssiBuff = new List<int>();
            _status = "Uninitialized";
            SelectedDeviceString = "Please select a device first.";
            // Width (in xamarin.forms units)
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            int xamarinWidth = (int)Math.Round(mainDisplayInfo.Width / mainDisplayInfo.Density);
            MaxRadiusSize = (int)Math.Round(xamarinWidth * MaxRadiusRelativeToScreen);
            initializeCircleSizes();
            OpenInfoPageCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(InfoPage)}"));
            SelectDevice = new Command(async () => await Shell.Current.GoToAsync($"{nameof(ItemsPage)}"));
        }

        private void initializeCircleSizes()
        {            
            // This list contains the sizes of the circles displayed on the device, ordered DESC.
            _circleSizes = new List<int>();
            _circleSizes.Add(MaxRadiusSize);
            for (int i = 1; i <= 4; i++)
            {
                double circleSize = MaxRadiusSize - (((MaxRadiusSize - MinRadiusSize) / 5) * i);
                // Make sure the circle an even size
                int circleSizeAsInt = Math.Floor(circleSize) % 2 == 0 ? (int) Math.Floor(circleSize) : (int) Math.Ceiling(circleSize);
                _circleSizes.Add(circleSizeAsInt);
            }
            _circleSizes.Add(MinRadiusSize);
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

        public List<int> CircleSizes
        {
            get => _circleSizes;
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
                Meter = rssiToMeter(CurrentRssi, CurrentTxPower);
            }
        }

        public int CurrentTxPower
        {
            get => _txPower;
            set
            {
                SetProperty(ref _txPower, value);
            }
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public string SelectedDeviceString
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        public Command OpenInfoPageCommand { get; }
        public Command SelectDevice { get; }

        public void OnAppearing()
        {
            if (App.DevicesStore.SelectedDevice is null)
            {
                Status = "No device selected!\nPlease select a device to continue.";
            }
            else
            {
                SelectedDeviceString = "" + App.DevicesStore.SelectedDevice.UserLabel + "\nClick to select another device.";
                Status = "Connecting to \"" + App.DevicesStore.SelectedDevice.UserLabel + "\"...\n" +
                    "If this takes longer than a few seconds, the device is probably out of range or turned off.";
                App.Bluetooth.StartRssiPolling(App.DevicesStore.SelectedDevice.BT_GUID, (int v, int txPower) =>
                {
                    CurrentTxPower = txPower;
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
                            "The current distance is about " + Meter.ToString("0.0") + " m.";
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
            int radiusScaleSize = MaxRadiusSize - MinRadiusSize;
            double inputScaleSize = scaleMax - scaleMin;

            double relScalePosition = (value - scaleMin) / inputScaleSize;
            int resultingRadius = MinRadiusSize + (int)(relScalePosition * radiusScaleSize);
            return resultingRadius;
        }

        private double rssiToMeter(int rssi, int measuredPower, double environmentalFactor = Constants.RssiEnvironmentalDefault)
        {
            // https://medium.com/beingcoders/convert-rssi-value-of-the-ble-bluetooth-low-energy-beacons-to-meters-63259f307283 
            return Math.Pow(10, (double)(measuredPower - rssi) / (10 * environmentalFactor));
        }
    }
}