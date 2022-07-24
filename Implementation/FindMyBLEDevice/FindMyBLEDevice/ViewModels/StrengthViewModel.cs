// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using System;
using System.Collections.Generic;
using FindMyBLEDevice.Services.Settings;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.XamarinAccess;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Bluetooth;
using Xamarin.Forms;
using FindMyBLEDevice.Services.Geolocation;

namespace FindMyBLEDevice.ViewModels
{
    public class StrengthViewModel : BaseViewModel
    {
        public const double MaxRadiusRelativeToScreen = 0.9;

        private readonly IDevicesStore devicesStore;
        private readonly IBluetooth bluetooth;
        private readonly IGeolocation geolocation;
        private readonly ISettings settings;
        private readonly int minRadiusSize;
        private readonly int maxRadiusSize;
        private readonly double meterScaleMin;
        private readonly double meterScaleMax;
        private readonly string _message = "'Strength Search' measures the distance to your lost device based on the emitting Bluetooth signal.\n"
                    + "By moving around, the blue circle radius changes.\n"
                    + "If you move away from your device, the circle radius will increase.\n"
                    + "If you approach your device, the circle radius will decrease.";

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

        private RssiBuffer _rssiBuffer;
        public double CurrentRssi
        {
            get => _rssiBuffer.Get();
            set
            {
                _rssiBuffer.Update(value);
                OnPropertyChanged(nameof(CurrentRssi));
                Meter = Bluetooth.RssiToMeter(CurrentRssi, CurrentTxPower);
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

        public Command ShowInfoPage { get; }
        public Command SelectDevice { get; }

        public StrengthViewModel(
            IDeviceDisplayAccess displayAccess, 
            IDevicesStore devicesStore, 
            IBluetooth bluetooth, 
            INavigator navigator, 
            IGeolocation geolocation, 
            ISettings settings)
        {
            Title = "Strength Search";

            this.devicesStore = devicesStore;
            this.bluetooth = bluetooth;
            this.geolocation = geolocation;
            this.settings = settings;

            meterScaleMin = Bluetooth.RssiToMeter(0, Constants.TxPowerDefault);
            meterScaleMax = Bluetooth.RssiToMeter(-100, Constants.TxPowerDefault);
            _rssiBuffer = new RssiBuffer();
            _currentTxPower = Constants.TxPowerDefault;
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
            if (settings.Get(SettingsNames.IncorporateGpsIntoRssi, Constants.IncorporateGpsIntoRssiDefault))
            {
                _rssiBuffer = new RssiExponentialBuffer(geolocation);
            }
            else
            {
                _rssiBuffer = new RssiRingBuffer(settings);
            }

            if (devicesStore.SelectedDevice is null)
            {
                SelectedDeviceString = "No device selected!\n> Click to select a device <";
                Status = "No device selected!\n> Please select a device to continue<";

            }
            else
            {
                SelectedDeviceString = "" + devicesStore.SelectedDevice.UserLabel + "\n> Click to select a different device <";
                Status = "Connecting to \"" + devicesStore.SelectedDevice.UserLabel + "\"...\n" +
                    "If this takes longer than a few seconds, the device is probably out of range or turned off.";
                bluetooth.StartRssiPolling(devicesStore.SelectedDevice.BT_GUID, (int newRssi) =>
                {
                    CurrentRssi = newRssi;

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
                }, (int txPower) =>
                {
                    CurrentTxPower = txPower;
                    Status = "Connected to \"" + devicesStore.SelectedDevice.UserLabel + "\".\n" +
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

        private int OtherScaleToRadius(double scaleMin, double scaleMax, double value)
        {
            int radiusScaleSize = maxRadiusSize - minRadiusSize;
            double inputScaleSize = scaleMax - scaleMin;

            double relScalePosition = (value - scaleMin) / inputScaleSize;
            int resultingRadius = minRadiusSize + (int)(relScalePosition * radiusScaleSize);
            return resultingRadius;
        }

        public void OnDisappearing()
        {
            bluetooth.StopRssiPolling();
        }
    }
}
