// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

namespace FindMyBLEDevice.Services.Permission
{
    public interface IPermission
    {
        bool CheckLocationPermission();
        bool CheckBluetoothPermission();
    }
}
