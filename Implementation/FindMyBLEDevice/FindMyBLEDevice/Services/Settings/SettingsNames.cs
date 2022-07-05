// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyBLEDevice.Services.Settings
{
    public static class SettingsNames
    {
        public const string DisplayNamelessDevices = "DisplayNamelessDevices";
        public const string DisplayWeakDevices = "DisplayWeakDevices";

        public const string RssiInterval = "RssiInterval";

        public const string UpdateServiceInterval = "UpdateServiceInterval";
    }
}
