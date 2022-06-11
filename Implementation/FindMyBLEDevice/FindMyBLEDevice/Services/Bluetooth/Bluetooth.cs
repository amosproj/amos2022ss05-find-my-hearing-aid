﻿// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using FindMyBLEDevice.Models;
using Plugin.BLE;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Threading;
using Plugin.BLE.Abstractions;
using FindMyBLEDevice.Exceptions;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public class Bluetooth : IBluetooth
    {
        
        private readonly IAdapter adapter;

        private Timer rssiPollingTimer;

        private int RssiPollInterval { get; }

        public Bluetooth(IAdapter adapter)
        {
            this.adapter = adapter;
            RssiPollInterval = 1000;
        }
        public Bluetooth() : this(CrossBluetoothLE.Current.Adapter) {}

        public async Task Search(int scanTimeout, ObservableCollection<BTDevice> availableDevices, Predicate<BTDevice> filter)
        {

            adapter.DeviceDiscovered += (s, a) => {
                Console.WriteLine(a.Device.Id);
                if (availableDevices.ToList<BTDevice>().Exists(o => Guid.Parse(o.BT_GUID) == a.Device.Id))
                {
                    return;
                }

                if (a.Device.Rssi < -80 || a.Device.Name is null)
                {
                    return;
                }

                BTDevice device = (new BTDevice()
                {
                    AdvertisedName = a.Device.Name,
                    BT_GUID = a.Device.Id.ToString()
                });

                if (filter == null || filter(device))
                {
                    availableDevices.Add(device);
                }
            };            

            adapter.ScanTimeout = scanTimeout;
            await adapter.StartScanningForDevicesAsync();
        }

        public async Task StopSearch()
        {
            if (adapter.IsScanning)
            {
                await adapter.StopScanningForDevicesAsync();
            }
        }
        
        public async Task StartRssiPolling(String btguid, Func<int, int> updateRssi)
        {
            try
            {
                IDevice device = await adapter.ConnectToKnownDeviceAsync(Guid.Parse(btguid));

                rssiPollingTimer = new Timer(async (o) => {

                    await device.UpdateRssiAsync();
                    updateRssi.Invoke(device.Rssi);

                }, "", 0, RssiPollInterval);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void StopRssiPolling()
        {
            rssiPollingTimer?.Dispose();
        }
    }
}
