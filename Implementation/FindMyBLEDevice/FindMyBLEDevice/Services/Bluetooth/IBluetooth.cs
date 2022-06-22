// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using FindMyBLEDevice.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public interface IBluetooth
    {
        Task Search(int scanTimeout, ObservableCollection<BTDevice> availableDevices, Predicate<BTDevice> filter);
        void StartRssiPolling(String btguid, Action<int, int> updateRssi, Action connected = null, Action disconnected = null);
        void StopRssiPolling();
        Task StopSearch();
        bool IsEnabled();
    }
}