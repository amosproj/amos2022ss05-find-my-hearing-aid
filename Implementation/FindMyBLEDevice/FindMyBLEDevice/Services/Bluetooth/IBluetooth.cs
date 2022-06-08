﻿// SPDX-License-Identifier: MIT
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
        Task Search(int scanTimeout, ObservableCollection<AvailableBTDevice> availableDevices, Predicate<AvailableBTDevice> filter);
        Task StartRssiPolling(string btguid, Func<int, int> updateRssi);
        void StopRssiPolling();
        Task StopSearch();
        bool IsEnabled();
    }
}