// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Models;
using Plugin.BLE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public class Bluetooth
    {
        private readonly List<AvailableBTDevice> deviceList;

        public Bluetooth()
        {
            deviceList = new List<AvailableBTDevice>();
        }

        public async Task Search(int scanTimeout)
        {

            deviceList.Clear();
            Plugin.BLE.Abstractions.Contracts.IAdapter adapter = CrossBluetoothLE.Current.Adapter;

            adapter.DeviceDiscovered += (s, a) => {

                if(!deviceList.Exists(o => o.Id == a.Device.Id))
                {
                    deviceList.Add(new AvailableBTDevice()
                    {
                        Name = a.Device.Name ?? "Unknown Device",
                        Id = a.Device.Id,
                        Rssi = a.Device.Rssi
                    });
                }

            };

            adapter.ScanTimeout = scanTimeout;
            await adapter.StartScanningForDevicesAsync();

        }

        public List<AvailableBTDevice> GetAvailableDevices()
        {
            return deviceList;
        }
    }
}
