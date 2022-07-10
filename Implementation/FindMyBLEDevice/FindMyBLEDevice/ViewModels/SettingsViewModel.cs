// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Settings;
using FindMyBLEDevice.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel() 
        { 
            _rssiIntervalString = RssiInterval.ToString();
            OpenInfoPageCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(InfoPage)}"));
        }

        public bool DisplayNamelessDevices
        {
            get => Preferences.Get(SettingsNames.DisplayNamelessDevices, false);
            set
            {
                Preferences.Set(SettingsNames.DisplayNamelessDevices, value);
                OnPropertyChanged(nameof(DisplayNamelessDevices));
            }
        }

        public bool DisplayWeakDevices
        {
            get => Preferences.Get(SettingsNames.DisplayWeakDevices, false);
            set
            {
                Preferences.Set(SettingsNames.DisplayWeakDevices, value);
                OnPropertyChanged(nameof(DisplayWeakDevices));
            }
        }

        public int RssiInterval
        {
            get => Preferences.Get(SettingsNames.RssiInterval, Constants.RssiIntervalDefault);
            set
            {
                Preferences.Set(SettingsNames.RssiInterval, value);
                OnPropertyChanged(nameof(RssiInterval));
                _rssiIntervalString = value.ToString();
                OnPropertyChanged(nameof(RssiIntervalString));
            }
        }
        private string _rssiIntervalString;

        public Command OpenInfoPageCommand { get; }

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


        public int UpdateServiceInterval
        {
            get => Preferences.Get(SettingsNames.UpdateServiceInterval, Constants.UpdateServiceIntervalDefault);
            set
            {
                Preferences.Set(SettingsNames.UpdateServiceInterval, value);
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
    }
}
