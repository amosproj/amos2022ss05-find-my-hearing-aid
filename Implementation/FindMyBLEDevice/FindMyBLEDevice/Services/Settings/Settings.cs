// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>

using Xamarin.Essentials;

namespace FindMyBLEDevice.Services.Settings
{
    public class Settings : ISettings
    {
        public string Get(string key, string defaultValue)
        {
            return Preferences.Get(key, defaultValue, null);
        }

        public bool Get(string key, bool defaultValue)
        {
            return Preferences.Get(key, defaultValue, null);
        }

        public int Get(string key, int defaultValue)
        {
            return Preferences.Get(key, defaultValue, null);
        }

        public double Get(string key, double defaultValue)
        {
            return Preferences.Get(key, defaultValue, null);
        }

        public float Get(string key, float defaultValue)
        {
            return Preferences.Get(key, defaultValue, null);
        }

        public long Get(string key, long defaultValue)
        {
            return Preferences.Get(key, defaultValue, null);
        }

        public void Set(string key, string value)
        {
            Preferences.Set(key, value, null);
        }

        public void Set(string key, bool value)
        {
            Preferences.Set(key, value, null);
        }

        public void Set(string key, int value)
        {
            Preferences.Set(key, value, null);
        }

        public void Set(string key, double value)
        {
            Preferences.Set(key, value, null);
        }

        public void Set(string key, float value)
        {
            Preferences.Set(key, value, null);
        }

        public void Set(string key, long value)
        {
            Preferences.Set(key, value, null);
        }

    }
}
