// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>

using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyBLEDevice.Services.Settings
{
    public interface ISettings
    {
        //
        // Summary:
        //     Gets the value for a given key, or the default specified if the key does not
        //     exist.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   defaultValue:
        //     Default value to return if the key does not exist.
        //
        // Returns:
        //     Value for the given key, or the default if it does not exist.
        string Get(string key, string defaultValue);

        //
        // Summary:
        //     Gets the value for a given key, or the default specified if the key does not
        //     exist.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   defaultValue:
        //     Default value to return if the key does not exist.
        //
        // Returns:
        //     Value for the given key, or the default if it does not exist.
        bool Get(string key, bool defaultValue);

        //
        // Summary:
        //     Gets the value for a given key, or the default specified if the key does not
        //     exist.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   defaultValue:
        //     Default value to return if the key does not exist.
        //
        // Returns:
        //     Value for the given key, or the default if it does not exist.
        int Get(string key, int defaultValue);

        //
        // Summary:
        //     Gets the value for a given key, or the default specified if the key does not
        //     exist.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   defaultValue:
        //     Default value to return if the key does not exist.
        //
        // Returns:
        //     Value for the given key, or the default if it does not exist.
        double Get(string key, double defaultValue);

        //
        // Summary:
        //     Gets the value for a given key, or the default specified if the key does not
        //     exist.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   defaultValue:
        //     Default value to return if the key does not exist.
        //
        // Returns:
        //     Value for the given key, or the default if it does not exist.
        float Get(string key, float defaultValue);

        //
        // Summary:
        //     Gets the value for a given key, or the default specified if the key does not
        //     exist.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   defaultValue:
        //     Default value to return if the key does not exist.
        //
        // Returns:
        //     Value for the given key, or the default if it does not exist.
        long Get(string key, long defaultValue);

        //
        // Summary:
        //     Sets a value for a given key.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   value:
        //     Preference value.
        void Set(string key, string value);

        //
        // Summary:
        //     Sets a value for a given key.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   value:
        //     Preference value.
        void Set(string key, bool value);

        //
        // Summary:
        //     Sets a value for a given key.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   value:
        //     Preference value.
        void Set(string key, int value);

        //
        // Summary:
        //     Sets a value for a given key.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   value:
        //     Preference value.
        void Set(string key, double value);

        //
        // Summary:
        //     Sets a value for a given key.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   value:
        //     Preference value.
        void Set(string key, float value);

        //
        // Summary:
        //     Sets a value for a given key.
        //
        // Parameters:
        //   key:
        //     Preference key.
        //
        //   value:
        //     Preference value.
        void Set(string key, long value);

    }
}
