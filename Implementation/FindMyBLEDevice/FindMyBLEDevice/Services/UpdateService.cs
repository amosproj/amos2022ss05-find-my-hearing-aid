// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Geolocation;
using FindMyBLEDevice.Services.Settings;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services
{
    public class UpdateService
    {
        private readonly IBluetooth bluetooth;
        private readonly IDevicesStore devicesStore;
        private readonly IGeolocation geolocation;

        private List<BTDevice> savedDevices;

        private bool running;

        public UpdateService(IBluetooth bluetooth, IDevicesStore deviceStore, IGeolocation geolocation)
        {
            this.bluetooth = bluetooth;
            this.devicesStore = deviceStore;
            this.geolocation = geolocation;

            savedDevices = new List<BTDevice>();
            devicesStore.DevicesChanged += OnSavedDevicesStoreChanged;

            running = false;
        }

        public UpdateService() : this(App.Bluetooth, App.DevicesStore, App.Geolocation) { }


        private async void OnSavedDevicesStoreChanged(object sender, EventArgs e)
        {
            savedDevices = await devicesStore.GetAllDevices();
        }

        public void Start()
        {
            if (running) return;

            running = true;
            Task.Run(async () =>
            {
                savedDevices = await devicesStore.GetAllDevices();
                while (true)
                {
                    // start connecting to all known devices
                    Dictionary<BTDevice, Task<IDevice>> reachableTasks = new Dictionary<BTDevice, Task<IDevice>>();
                    foreach (BTDevice device in savedDevices)
                    {
                        try
                        {
                            reachableTasks.Add(device, bluetooth.DeviceReachableAsync(device));
                        } catch (Exception e)
                        {
                            Console.WriteLine($"[UpdateService] {DateTime.Now} Checking if reachable failed for: {device.UserLabel}");
                        }
                    }

                    // wait until all connection attempts finished (adapter timeout ~5s)
                    await Task.WhenAll(reachableTasks.Values);

                    // update geolocations of reachable devices
                    var location = await geolocation.GetCurrentLocation();
                    foreach (KeyValuePair<BTDevice, Task<IDevice>> p in reachableTasks)
                    {
                        var databaseDevice = p.Key;
                        var adapterDeviceTask = p.Value;
                        if (adapterDeviceTask.Result is null || adapterDeviceTask.Result.Rssi < Constants.RssiTooFarThreshold)
                        {
                            Console.WriteLine($"[UpdateService] {DateTime.Now} Out of reach: {databaseDevice.UserLabel}");
                            databaseDevice.WithinRange = false;
                        } else
                        {
                            Console.WriteLine($"[UpdateService] {DateTime.Now} Reachable: {databaseDevice.UserLabel}");
                            databaseDevice.LastGPSLatitude = location.Latitude;
                            databaseDevice.LastGPSLongitude = location.Longitude;
                            databaseDevice.LastGPSTimestamp = DateTime.Now;
                            databaseDevice.WithinRange = true;
                        }
                        await devicesStore.UpdateDevice(databaseDevice);
                    }

                    // delay next poll
                    const int pollPeriod = 5 * 1000; 
                    await Task.Delay(pollPeriod);
                }
            });
        }
    }
}
