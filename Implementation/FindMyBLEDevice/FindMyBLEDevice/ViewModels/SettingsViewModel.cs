﻿// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Settings;

namespace FindMyBLEDevice.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettings settings;

        public SettingsViewModel(ISettings settings)
        {
            this.settings = settings;

            _rssiIntervalString = RssiInterval.ToString();
            _updateServiceIntervalString = UpdateServiceInterval.ToString();
        }

        public bool DisplayNamelessDevices
        {
            get => settings.Get(SettingsNames.DisplayNamelessDevices, false);
            set
            {
                settings.Set(SettingsNames.DisplayNamelessDevices, value);
                OnPropertyChanged(nameof(DisplayNamelessDevices));
            }
        }

        public bool DisplayWeakDevices
        {
            get => settings.Get(SettingsNames.DisplayWeakDevices, false);
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
    }
}
