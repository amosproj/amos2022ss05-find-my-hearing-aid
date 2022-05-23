using FindMyBLEDevice.Models;
using Plugin.BLE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public class Bluetooth
    {
        private List<AvailableBTDevice> deviceList;

        public Bluetooth()
        {
            deviceList = new List<AvailableBTDevice>();
        }

        public async Task search(int scanTimeout)
        {
            deviceList = new List<AvailableBTDevice>();
            var adapter = CrossBluetoothLE.Current.Adapter;

            adapter.DeviceDiscovered += async (s, a) => {
                deviceList.Add(new AvailableBTDevice()
                {
                    Name = a.Device.Name,
                    Id = a.Device.Id,
                    Rssi = a.Device.Rssi
                });
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
