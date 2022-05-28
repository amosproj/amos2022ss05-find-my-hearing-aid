// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Models;
using Plugin.BLE;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Plugin.BLE.Abstractions.Contracts;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public class Bluetooth
    {
        
        private readonly IAdapter adapter;

        public Bluetooth(IAdapter adapter)
        {
            this.adapter = adapter; 
        }

        public Bluetooth()
        {
            adapter = CrossBluetoothLE.Current.Adapter;
        }

        public async Task Search(int scanTimeout, ObservableCollection<AvailableBTDevice> availableDevices)
        {

            adapter.DeviceDiscovered += (s, a) => {

                if (availableDevices.ToList<AvailableBTDevice>().Exists(o => o.Id == a.Device.Id))
                {
                    return;
                }
                                
                if (a.Device.Rssi < -80 || a.Device.Name is null)
                {
                    // return;
                }

                availableDevices.Add(new AvailableBTDevice()
                {
                    Name = a.Device.Name,
                    Id = a.Device.Id,
                    Rssi = a.Device.Rssi
                });               

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
        
    }
}
