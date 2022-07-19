// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Settings;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettings settings;
        private readonly IDevicesStore devicesStore;

        public Command<string> OpenUrl { get; }
        public Command DefaultButtonTapped { get; }
        public Command RemoveDevicesButtonTapped { get; }

        public SettingsViewModel(ISettings settings, IDevicesStore devicesStore)
        {
            Title = "Settings";

            this.settings = settings;
            this.devicesStore = devicesStore;

            _rssiIntervalString = RssiInterval.ToString();
            _updateServiceIntervalString = UpdateServiceInterval.ToString();

            OpenUrl = new Command<string>(async (url) => await Launcher.OpenAsync(url));
            DefaultButtonTapped = new Command(async () => await RestoreDefaults());
            RemoveDevicesButtonTapped = new Command(async () => await RemoveAllDevices());
        }

        public bool DisplayNamelessDevices
        {
            get => settings.Get(SettingsNames.DisplayNamelessDevices, Constants.DisplayNamelessDevicesDefault);
            set
            {
                settings.Set(SettingsNames.DisplayNamelessDevices, value);
                OnPropertyChanged(nameof(DisplayNamelessDevices));
            }
        }

        public bool DisplayWeakDevices
        {
            get => settings.Get(SettingsNames.DisplayWeakDevices, Constants.DisplayWeakDevicesDefault);
            set
            {
                settings.Set(SettingsNames.DisplayWeakDevices, value);
                OnPropertyChanged(nameof(DisplayWeakDevices));
            }
        }

        public int RssiInterval
        {
            get => settings.Get(SettingsNames.RssiInterval, Constants.RssiIntervalDefault);
            set
            {
                settings.Set(SettingsNames.RssiInterval, value);
                OnPropertyChanged(nameof(RssiInterval));
                _rssiIntervalString = value.ToString();
                OnPropertyChanged(nameof(RssiIntervalString));
            }
        }

        private string _rssiIntervalString;
        public string RssiIntervalString
        {
            get => _rssiIntervalString;
            set
            {
                if(_rssiIntervalString != value)
                {
                    bool parsable = int.TryParse(value, out int result);
                    if (string.IsNullOrEmpty(value))
                    {
                        RssiInterval = RssiIntervalDefault;
                        _rssiIntervalString = value;
                    }
                    else if (parsable)
                    {
                        RssiInterval = result;
                    }
                    OnPropertyChanged(nameof(RssiIntervalString));
                }
            }
        }

        public int RssiIntervalDefault
        {
            get => Constants.RssiIntervalDefault;
        }

        public int RssiIntervalMin
        {
            get => Constants.RssiIntervalMin;
        }

        public int RssiIntervalMax
        {
            get => Constants.RssiIntervalMax;
        }

        public bool IncorporateGpsIntoRssi
        {
            get => settings.Get(SettingsNames.IncorporateGpsIntoRssi, Constants.IncorporateGpsIntoRssiDefault);
            set
            {
                settings.Set(SettingsNames.IncorporateGpsIntoRssi, value);
                OnPropertyChanged(nameof(IncorporateGpsIntoRssi));
            }
        }


        public int UpdateServiceInterval
        {
            get => settings.Get(SettingsNames.UpdateServiceInterval, Constants.UpdateServiceIntervalDefault);
            set
            {
                settings.Set(SettingsNames.UpdateServiceInterval, value);
                OnPropertyChanged(nameof(UpdateServiceInterval));
                _updateServiceIntervalString = value.ToString();
                OnPropertyChanged(nameof(UpdateServiceIntervalString));
            }
        }

        private string _updateServiceIntervalString;
        public string UpdateServiceIntervalString
        {
            get => _updateServiceIntervalString;
            set
            {
                if (_updateServiceIntervalString != value)
                {
                    bool parsable = int.TryParse(value, out int result);
                    if (string.IsNullOrEmpty(value))
                    {
                        UpdateServiceInterval = UpdateServiceIntervalDefault;
                        _updateServiceIntervalString = value;
                    }
                    else if (parsable)
                    {
                        UpdateServiceInterval = result;
                    }
                    OnPropertyChanged(nameof(UpdateServiceIntervalString));
                }
            }
        }

        public int UpdateServiceIntervalDefault
        {
            get => Constants.UpdateServiceIntervalDefault;
        }

        public int UpdateServiceIntervalMin
        {
            get => Constants.UpdateServiceIntervalMin;
        }

        public int UpdateServiceIntervalMax
        {
            get => Constants.UpdateServiceIntervalMax;
        }

        private async Task RestoreDefaults()
        {
            // Show confirmation dialog
            bool answer = await Application.Current.MainPage.DisplayAlert(
                "Restore Default Settings", 
                "Are yout sure you want to restore all default settings?", 
                "Yes", "Cancel");
            if (answer)
            {
                DisplayNamelessDevices = Constants.DisplayNamelessDevicesDefault;
                DisplayWeakDevices = Constants.DisplayWeakDevicesDefault;
                RssiInterval = Constants.RssiIntervalDefault;
                IncorporateGpsIntoRssi = Constants.IncorporateGpsIntoRssiDefault;
                UpdateServiceInterval = Constants.UpdateServiceIntervalDefault;
            }
        }

        private async Task RemoveAllDevices()
        {
            // Show confirmation dialog
            bool answer = await Application.Current.MainPage.DisplayAlert(
                "Remove All Devices", 
                "Are yout sure you want to remove all devices?", 
                "Yes", "Cancel");
            if (answer)
            {
                var devices = await devicesStore.GetAllDevices();
                foreach (var device in devices)
                {
                    await devicesStore.DeleteDevice(device.ID);
                }
            }
        }
    }
}
