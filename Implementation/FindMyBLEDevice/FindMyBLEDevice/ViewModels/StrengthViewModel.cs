// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using FindMyBLEDevice.Services.Settings;
using FindMyBLEDevice.Services;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.XamarinAccess;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Bluetooth;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class StrengthViewModel : BaseViewModel
    {
        public const double MaxRadiusRelativeToScreen = 0.9;

        private readonly IDevicesStore devicesStore;
        private readonly IBluetooth bluetooth;
        private readonly int minRadiusSize;
        private readonly int maxRadiusSize;
        private readonly double meterScaleMin;
        private readonly double meterScaleMax;
        private readonly List<int> rssiBuff;
        private readonly string _message = "'Strength Search' measures the distance to your lost device based on the emitting Bluetooth signal.\n"
                    + "By moving around, the blue circle radius changes.\n"
                    + "If you move away from your device, the circle radius will increase.\n"
                    + "If you approach your device, the circle radius will decrease.";


        public Command ShowInfoPage { get; }
        public Command SelectDevice { get; }

        public BTDevice Device => devicesStore.SelectedDevice;

        private List<int> _circleSizes; 
        public List<int> CircleSizes
        {
            get => _circleSizes;
        }

        private int _radius;
        public int Radius
        {
            get => _radius;
            set => SetProperty(ref _radius, value);
        }

        private double _meter;
        public double Meter
        {
            get => _meter;
            set
            {
                SetProperty(ref _meter, value);
                Radius = OtherScaleToRadius(meterScaleMin, meterScaleMax, Meter);
            }
        }

        private int _currentRssi;
        public int CurrentRssi
        {
            get => _currentRssi;
            set
            {
                SetProperty(ref _currentRssi, value);
                Meter = RssiToMeter(CurrentRssi, CurrentTxPower);
            }
        }

        private int _currentTxPower;
        public int CurrentTxPower
        {
            get => _currentTxPower;
            set
            {
                SetProperty(ref _currentTxPower, value);
            }
        }
        private string _status;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private string _selectedDeviceString;
        public string SelectedDeviceString
        {
            get => _selectedDeviceString;
            set => SetProperty(ref _selectedDeviceString, value);
        }

        public StrengthViewModel(IDeviceDisplayAccess displayAccess, IDevicesStore devicesStore, IBluetooth bluetooth, INavigator navigator)
        {
            Title = "StrengthSearch";

            this.devicesStore = devicesStore;
            this.bluetooth = bluetooth;

            meterScaleMin = RssiToMeter(0, Constants.TxPowerDefault);
            meterScaleMax = RssiToMeter(-100, Constants.TxPowerDefault);
            rssiBuff = new List<int>();
            _status = "Uninitialized";

            // Width (in xamarin.forms units)
            int xamarinWidth = (int)Math.Round(displayAccess.Width / displayAccess.Density);
            minRadiusSize = 30;
            maxRadiusSize = (int)Math.Round(xamarinWidth * MaxRadiusRelativeToScreen);
            InitializeCircleSizes();

            SelectDevice = new Command(
                async () => await navigator.GoToAsync(navigator.DevicesPage));
            ShowInfoPage = new Command(
                async () => await App.Current.MainPage.DisplayAlert("Information", _message, "Ok"));
        }

        private void InitializeCircleSizes()
        {
            // This list contains the sizes of the circles displayed on the device, ordered DESC.
            _circleSizes = new List<int>
            {
                maxRadiusSize
            };
            for (int i = 1; i <= 4; i++)
            {
                double circleSize = maxRadiusSize - (((maxRadiusSize - minRadiusSize) / 5) * i);
                // Make sure the circle an even size
                int circleSizeAsInt = Math.Floor(circleSize) % 2 == 0 ? (int) Math.Floor(circleSize) : (int) Math.Ceiling(circleSize);
                _circleSizes.Add(circleSizeAsInt);
            }
            _circleSizes.Add(minRadiusSize);
        }

        public void OnAppearing()
        {
            await CheckBluetoothAndLocation.Check();

            if (devicesStore.SelectedDevice is null)
            {
                Status = "No device selected!\nPlease select a device to continue.";
            }
            else
            {
                SelectedDeviceString = "" + devicesStore.SelectedDevice.UserLabel + "\n> Click to select a different device <";
                Status = "Connecting to \"" + devicesStore.SelectedDevice.UserLabel + "\"...\n" +
                    "If this takes longer than a few seconds, the device is probably out of range or turned off.";
                bluetooth.StartRssiPolling(devicesStore.SelectedDevice.BT_GUID, (int v, int txPower) =>
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
                        Status = "\"" + devicesStore.SelectedDevice.UserLabel + "\" is very close!\n"+
                            "Try searching the vicinity to find it.";
                    }
                    else
                    {
                        Status = "Connected to \"" + devicesStore.SelectedDevice.UserLabel + "\".\n" +
                            "The current distance is about " + Meter.ToString("0.0") + " m.";
                    }
                }, () =>
                {
                    Status = "Connected to \"" + devicesStore.SelectedDevice.UserLabel + "\".\n" +
                        "Move around to see in which direction the signal gets better.";
                }, () =>
                {
                    Status = "Disconnected! Trying to reconnect...\n" +
                        "Please move back into range of the device.";
                    CurrentRssi = -100;
                    Meter = meterScaleMax;
                });
            } else
            {
                SelectedDeviceString = "No device selected!\n> Click to select a device <";
            }
        }

        /// <summary>
        /// Calculates the strengths views radius from any given input value and its scale.
        /// </summary>
        /// <param name="scaleMin">Min value of the input value's scale.</param>
        /// <param name="scaleMax">Max value of the input value's scale.</param>
        /// <param name="value">Input value</param>
        /// <returns>circle radius</returns>
        private int OtherScaleToRadius(double scaleMin, double scaleMax, double value)
        {
            int radiusScaleSize = maxRadiusSize - minRadiusSize;
            double inputScaleSize = scaleMax - scaleMin;

            double relScalePosition = (value - scaleMin) / inputScaleSize;
            int resultingRadius = minRadiusSize + (int)(relScalePosition * radiusScaleSize);
            return resultingRadius;
        }

        private double RssiToMeter(int rssi, int measuredPower, double environmentalFactor = Constants.RssiEnvironmentalDefault)
        {
            // https://medium.com/beingcoders/convert-rssi-value-of-the-ble-bluetooth-low-energy-beacons-to-meters-63259f307283 
            return Math.Pow(10, (double)(measuredPower - rssi) / (10 * environmentalFactor));
        }

        public void OnDisappearing()
        {
            bluetooth.StopRssiPolling();
        }
    }
}
