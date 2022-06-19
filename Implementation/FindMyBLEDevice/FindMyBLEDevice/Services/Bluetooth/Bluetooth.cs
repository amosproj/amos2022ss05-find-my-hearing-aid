// SPDX-License-Identifier: MIT
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
using Xamarin.Essentials;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public class Bluetooth : IBluetooth
    {
        private const int PollDelay = 25;
        
        private readonly IAdapter adapter;

        private CancellationTokenSource rssiCancel;

        public Bluetooth(IAdapter adapter)
        {
            this.adapter = adapter;
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

                if (a.Device.Rssi < -80 && !Preferences.Get(PreferenceNames.DisplayWeakDevices, false))
                {
                    return;
                }

                if (a.Device.Name is null && !Preferences.Get(PreferenceNames.DisplayNamelessDevices, false))
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
        
        public void StartRssiPolling(String btguid, Action<int> updateRssi, Action connected = null, Action disconnected = null)
        {
            StopRssiPolling();
            rssiCancel = new CancellationTokenSource();
            var token = rssiCancel.Token;
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        IDevice device = await adapter.ConnectToKnownDeviceAsync(Guid.Parse(btguid));

                        if (!(connected is null)) connected.Invoke();

                        try
                        {
                            while((!token.IsCancellationRequested) && device.State == DeviceState.Connected)
                            {
                                await device.UpdateRssiAsync();
                                updateRssi.Invoke(device.Rssi);
                                await Task.Delay(PollDelay);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        if (!(disconnected is null)) disconnected.Invoke();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }, token);
        }

        public void StopRssiPolling()
        {
            rssiCancel?.Cancel();
        }
    }
}
