using FindMyBLEDevice.Models;
using Plugin.BLE;
using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyBLEDevice.Services.Bluetooth
{
    class Bluetooth
    {

        public async void search()
        {

            var ble = CrossBluetoothLE.Current;
            var adapter = CrossBluetoothLE.Current.Adapter;

            adapter.DeviceDiscovered += async (s, a) => {

                string name = $"Name: {a.Device.Name} - RSSI: {a.Device.Rssi}";

                List<BTDevice> list = await App.DevicesStore.GetAllDevices();

                bool exists = false;
                foreach (BTDevice dev in list)
                {
                    if (dev.BT_id == a.Device.Id.ToString())
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    await App.DevicesStore.AddDevice(a.Device.Id.ToString(), name);
                }

            };

            adapter.ScanTimeout = 5000000;

            Console.WriteLine("Start scanning");
            await adapter.StartScanningForDevicesAsync();
            Console.WriteLine("End scanning");


        }




    }
}
