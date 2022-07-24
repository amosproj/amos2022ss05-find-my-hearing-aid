// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using FindMyBLEDevice.Models;
using System;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public interface IBluetooth
    {
        event EventHandler<BTDevice> DeviceDiscovered;
        Task StartSearch(int timeout);
        Task StopSearch();
        void StartRssiPolling(String btguid, Action<int> updateRssi, Action<int> connected = null, Action disconnected = null);
        void StopRssiPolling();
        Task<int> DeviceTXPowerAsync(String btguid);
        Task<int> DeviceReachableAsync(String btguid);
        bool IsEnabled();
    }
}
