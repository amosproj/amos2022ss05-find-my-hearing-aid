// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Settings;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services
{
    public class UpdateService
    {
        private readonly IAdapter adapter;
        private readonly IDevicesStore devicesStore;
        private readonly ISettings settings;

        private List<BTDevice> savedDevices;
        private readonly List<IDevice> discoveredDevices;

        private bool running;

        public UpdateService(IAdapter adapter, IDevicesStore deviceStore, ISettings settings)
        {
            this.adapter = adapter;
            this.devicesStore = deviceStore;
            this.settings = settings;

            savedDevices = new List<BTDevice>();
            devicesStore.DevicesChanged += OnSavedDevicesStoreChanged;

            discoveredDevices = new List<IDevice>();
            adapter.DeviceDiscovered += OnDeviceDiscovered;

            running = false;
        }

        public UpdateService() : this(CrossBluetoothLE.Current.Adapter, App.DevicesStore, App.Settings) { }


        private async void OnSavedDevicesStoreChanged(object sender, EventArgs e)
        {
            savedDevices = await devicesStore.GetAllDevices();
        }

        private async void OnDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            discoveredDevices.Add(e.Device);

            BTDevice savedDevice = savedDevices.Find(s => Guid.Parse(s.BT_GUID) == e.Device.Id);
            if(savedDevice != null)
            {
                Console.WriteLine("[UpdateService] Device \"" + savedDevice.UserLabel + "\" is within range at rssi " + e.Device.Rssi);
                savedDevice.WithinRange = true;
                var location = await App.Geolocation.GetCurrentLocation();
                savedDevice.LastGPSLongitude = location.Longitude;
                savedDevice.LastGPSLatitude = location.Latitude;
                await devicesStore.UpdateDevice(savedDevice);
            }
        }

        public void Start()
        {
            if (running) return;

            running = true;
            Task.Run(async () =>
            {
                savedDevices = await devicesStore.GetAllDevices();
                while(true)
                {
                    try
                    {
                        foreach(var s in savedDevices)
                        {
                            var s_Id = Guid.Parse(s.BT_GUID);
                            if(!discoveredDevices.Exists(d => d.Id == s_Id))
                            {
                                Console.WriteLine("[UpdateService] Device \"" + s.UserLabel + "\" is out of range");
                                s.WithinRange = false;
                                await devicesStore.UpdateDevice(s);
                            }
                        }

                        discoveredDevices.Clear();

                        Console.WriteLine("[UpdateService] Scanning...");
                        adapter.ScanTimeout = 1000 * settings.Get(SettingsNames.UpdateServiceInterval, Constants.UpdateServiceIntervalDefault);
                        await adapter.StartScanningForDevicesAsync();
                    } 
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            });
        }
    }
}
