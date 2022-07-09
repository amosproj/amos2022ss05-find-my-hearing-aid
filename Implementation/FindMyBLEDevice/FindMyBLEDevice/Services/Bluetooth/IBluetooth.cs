// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using FindMyBLEDevice.Models;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public interface IBluetooth
    {
        event EventHandler<BTDevice> DeviceDiscovered;
        Task StartSearch(int timeout);
        Task StopSearch();
        void StartRssiPolling(String btguid, Action<int, int> updateRssi, Action connected = null, Action disconnected = null);
        void StopRssiPolling();
        Task<IDevice> DeviceReachableAsync(BTDevice device);
        bool IsEnabled();
    }
}