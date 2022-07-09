// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Geolocation;
using FindMyBLEDevice.Services.Settings;
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
        private readonly ISettings settings;

#pragma warning disable S1450
        private List<BTDevice> savedDevices;
#pragma warning restore S1450

        private bool running;

        public UpdateService(IBluetooth bluetooth, IDevicesStore devicesStore, IGeolocation geolocation, ISettings settings)
        {
            this.bluetooth = bluetooth;
            this.devicesStore = devicesStore;
            this.geolocation = geolocation;
            this.settings = settings;

            savedDevices = new List<BTDevice>();
            this.devicesStore.DevicesChanged += OnSavedDevicesStoreChanged;

            running = false;
        }

        private async void OnSavedDevicesStoreChanged(object sender, EventArgs e)
        {
            savedDevices = await devicesStore.GetAllDevices();
        }

        public void Start()
        {
            if (running) return;

            running = true;
            Task.Run(RunAsync);
        }

        public async Task RunAsync()
        {
            try
            {
                savedDevices = await devicesStore.GetAllDevices();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            while (running)
            {
                try
                {
                    var startTime = DateTime.Now;

                    // run the service tasks
                    await UpdateDevices();
                    // to be determined: expose an event that is fired here
                    // if done so, UpdateDevices() should be made synchronous
                    // in order to make the update tick wait for UpdateDevices() to finish again

                    // delay next iteration
                    int remaining = (int)(startTime.AddSeconds(settings.Get(SettingsNames.UpdateServiceInterval, Constants.UpdateServiceIntervalDefault)) 
                        - DateTime.Now).TotalMilliseconds;
                    if (remaining > 0) await Task.Delay(remaining);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private async Task UpdateDevices()
        {
            if (savedDevices.Count == 0) return;

            // start connecting to all known devices
            Dictionary<BTDevice, Task<int>> reachableTasks = new Dictionary<BTDevice, Task<int>>();
            foreach (BTDevice device in savedDevices)
            {
                reachableTasks.Add(device, bluetooth.DeviceReachableAsync(device));
            }

            // wait until all connection attempts finished (adapter timeout ~5s)
            await Task.WhenAll(reachableTasks.Values);

            // update geolocations of reachable devices
            var location = await geolocation.GetCurrentLocation();
            foreach (KeyValuePair<BTDevice, Task<int>> p in reachableTasks)
            {
                var databaseDevice = p.Key;
                var adapterDeviceTask = p.Value;
                if (adapterDeviceTask.Result == int.MinValue || adapterDeviceTask.Result < Constants.RssiTooFarThreshold)
                {
                    databaseDevice.WithinRange = false;
                }
                else
                {
                    databaseDevice.LastGPSLatitude = location.Latitude;
                    databaseDevice.LastGPSLongitude = location.Longitude;
                    databaseDevice.LastGPSTimestamp = DateTime.Now;
                    databaseDevice.WithinRange = true;
                }
                await devicesStore.UpdateDevice(databaseDevice);
            }
        }
    }
}
