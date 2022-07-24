// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyBLEDevice.Services.Permission
{
    public interface IPermission
    {
        bool checkLocationPermission();
        bool checkBluetoothPermission();
    }
}
