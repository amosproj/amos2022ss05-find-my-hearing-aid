// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Geolocation;
using FindMyBLEDevice.Services.Settings;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.ForegroundService
{
    public class ForegroundService : IForegroundService
    {
        private readonly IBluetooth bluetooth;
        private readonly IDevicesStore deviceStore;
        private readonly IGeolocation geolocation;
        private readonly ISettings settings;

#pragma warning disable S1450
        private List<BTDevice> savedDevices;
#pragma warning restore S1450

        private bool running;

        public event EventHandler<ForegroundServiceEventArgs> ServiceIteration;

        public ForegroundService(IBluetooth bluetooth, IDevicesStore deviceStore, IGeolocation geolocation, ISettings settings)
        {
            this.bluetooth = bluetooth;
            this.deviceStore = deviceStore;
            this.geolocation = geolocation;
            this.settings = settings;

            savedDevices = new List<BTDevice>();
            this.deviceStore.DevicesChanged += OnSavedDevicesStoreChanged;

            ServiceIteration += UpdateDevices;

            running = false;
        }

        public ForegroundService() : this(App.Bluetooth, App.DevicesStore, App.Geolocation, App.Settings) { }


        private async void OnSavedDevicesStoreChanged(object sender, EventArgs e)
        {
            savedDevices = await deviceStore.GetAllDevices();
        }

        public void Start()
        {
            if (running) return;

            running = true;
            Task.Run(async () =>
            {
                savedDevices = await deviceStore.GetAllDevices();
                while (true)
                {
                    var ea = new ForegroundServiceEventArgs { Devices = savedDevices };
                    ServiceIteration?.Invoke(this, ea);

                    // delay next poll
                    await Task.Delay(1000 * settings.Get(SettingsNames.UpdateServiceInterval, Constants.UpdateServiceIntervalDefault));
                }
            });
        }

        private async void UpdateDevices(object sender, ForegroundServiceEventArgs e)
        {
            // start connecting to all known devices
            Dictionary<BTDevice, Task<IDevice>> reachableTasks = new Dictionary<BTDevice, Task<IDevice>>();
            foreach (BTDevice device in e.Devices)
            {
                reachableTasks.Add(device, bluetooth.DeviceReachableAsync(device));
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
                }
                else
                {
                    Console.WriteLine($"[UpdateService] {DateTime.Now} Reachable: {databaseDevice.UserLabel}");
                    databaseDevice.LastGPSLatitude = location.Latitude;
                    databaseDevice.LastGPSLongitude = location.Longitude;
                    databaseDevice.LastGPSTimestamp = DateTime.Now;
                    databaseDevice.WithinRange = true;
                }
                await deviceStore.UpdateDevice(databaseDevice);
            }

        }
    }
}
