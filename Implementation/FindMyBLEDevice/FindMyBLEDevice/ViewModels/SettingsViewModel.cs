// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel() 
        { 
            _rssiIntervalString = RssiInterval.ToString();
        }
        
        public bool DisplayNamelessDevices
        {
            get => Preferences.Get(PreferenceNames.DisplayNamelessDevices, false);
            set
            {
                Preferences.Set(PreferenceNames.DisplayNamelessDevices, value);
                OnPropertyChanged(nameof(DisplayNamelessDevices));
            }
        }

        public bool DisplayWeakDevices
        {
            get => Preferences.Get(PreferenceNames.DisplayWeakDevices, false);
            set
            {
                Preferences.Set(PreferenceNames.DisplayWeakDevices, value);
                OnPropertyChanged(nameof(DisplayWeakDevices));
            }
        }

        public int RssiInterval
        {
            get => Preferences.Get(PreferenceNames.RssiInterval, Constants.RssiIntervalDefault);
            set
            {
                Preferences.Set(PreferenceNames.RssiInterval, value);
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
    }
}
