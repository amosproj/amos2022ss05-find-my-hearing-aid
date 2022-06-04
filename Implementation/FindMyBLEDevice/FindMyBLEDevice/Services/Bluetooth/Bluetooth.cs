// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Models;
using Plugin.BLE;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Threading;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public class Bluetooth : IBluetooth
    {
        
        private readonly IAdapter adapter;

        private Timer rssiPollingTimer;

        public Bluetooth(IAdapter adapter)
        {
            this.adapter = adapter; 
        }

        public Bluetooth()
        {
            adapter = CrossBluetoothLE.Current.Adapter;
        }

        public async Task Search(int scanTimeout, ObservableCollection<AvailableBTDevice> availableDevices, Predicate<AvailableBTDevice> filter)
        {

            adapter.DeviceDiscovered += (s, a) => {
                Console.WriteLine(a.Device.Id);
                if (availableDevices.ToList<AvailableBTDevice>().Exists(o => o.Id == a.Device.Id))
                {
                    return;
                }

                if (a.Device.Rssi < -80 || a.Device.Name is null)
                {
                    return;
                }

                AvailableBTDevice device = (new AvailableBTDevice()
                {
                    Name = a.Device.Name,
                    Id = a.Device.Id,
                    Rssi = a.Device.Rssi
                });

                if (filter == null || !filter(device))
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

            IDevice device = await adapter.ConnectToKnownDeviceAsync(Guid.Parse(btguid));

            rssiPollingTimer = new Timer(async (o) => {

                await device.UpdateRssiAsync();
                updateRssi.Invoke(device.Rssi);

            }, "", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));           

        }

        public void StopRssiPolling()
        {
            rssiPollingTimer?.Dispose();
        }


    }
}
