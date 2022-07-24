// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using FindMyBLEDevice.Models;
using Plugin.BLE;
using System.Threading.Tasks;
using System.Linq;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Threading;
using Plugin.BLE.Abstractions;
using FindMyBLEDevice.Services.Settings;

namespace FindMyBLEDevice.Services.Bluetooth
{
    public class Bluetooth : IBluetooth
    {
        private readonly Guid TX_POWER_SERVICE = Guid.ParseExact("00001804-0000-1000-8000-00805f9b34fb", "d");
        private readonly Guid TX_POWER_LEVEL_CHARACTERISTIC = Guid.ParseExact("00002a07-0000-1000-8000-00805f9b34fb", "d");

        private readonly IAdapter adapter;
        private readonly ISettings settings;

        private CancellationTokenSource rssiCancel;

        public event EventHandler<BTDevice> DeviceDiscovered;

        public Bluetooth(IAdapter adapter, ISettings settings)
        {
            this.adapter = adapter;
            this.settings = settings;

            adapter.DeviceDiscovered += OnDeviceDiscovered;
        }
        public Bluetooth(ISettings settings) : this(CrossBluetoothLE.Current.Adapter, settings) {}

        private void OnDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            if (e.Device.Rssi < Constants.RssiTooFarThreshold 
                && !settings.Get(SettingsNames.DisplayWeakDevices, false))
            {
                return;
            }

            if (e.Device.Name is null 
                && !settings.Get(SettingsNames.DisplayNamelessDevices, false))
            {
                return;
            }

            DeviceDiscovered?.Invoke(this, new BTDevice()
            {
                AdvertisedName = e.Device.Name,
                BT_GUID = e.Device.Id.ToString()
            });
        }

        public async Task StartSearch(int timeout)
        {
            adapter.ScanTimeout = timeout;
            await adapter.StartScanningForDevicesAsync();
        }

        public async Task StopSearch()
        {
            if (adapter.IsScanning)
            {
                await adapter.StopScanningForDevicesAsync();
            }
        }
        
        public void StartRssiPolling(String btguid, Action<int> updateRssi, Action<int> connected = null, Action disconnected = null)
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

                        int txPower = await DeviceTXPowerAsync(device);
                        
                        if (!(connected is null)) connected.Invoke(txPower);

                        try
                        {
                            while((!token.IsCancellationRequested) && device.State == DeviceState.Connected)
                            {
                                await device.UpdateRssiAsync();
                                updateRssi.Invoke(device.Rssi);
                                await Task.Delay(settings.Get(SettingsNames.RssiInterval, Constants.RssiIntervalDefault));
                            }
                            await adapter.DisconnectDeviceAsync(device);
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
        
        public async Task<int> DeviceTXPowerAsync(String btguid)
        {
            try
            {
                IDevice device = await adapter.ConnectToKnownDeviceAsync(Guid.Parse(btguid));
                return await DeviceTXPowerAsync(device);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return Constants.TxPowerDefault;
        }

        private async Task<int> DeviceTXPowerAsync(IDevice device)
        {
            try
            {
                var service = await device.GetServiceAsync(TX_POWER_SERVICE);
                if (service != null)
                {
                    var characteristic = await service.GetCharacteristicAsync(TX_POWER_LEVEL_CHARACTERISTIC);
                    if (characteristic != null)
                    {
                        var value = await characteristic.ReadAsync();
                        if (value != null)
                        {
                            return Convert.ToInt32((sbyte)value[0]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return Constants.TxPowerDefault;
        }

        public async Task<int> DeviceReachableAsync(BTDevice device)
        {
            IDevice adapterDevice = null;
            var connDevMatchingGuid = adapter.ConnectedDevices.Where(connDev => connDev.Id.ToString() == device.BT_GUID);
            if (connDevMatchingGuid.Any())
            {
                adapterDevice = connDevMatchingGuid.First();
            } 
            else
            {
                try
                {
                    ConnectParameters par = new ConnectParameters(autoConnect: false, forceBleTransport: true);
                    adapterDevice = await adapter.ConnectToKnownDeviceAsync(Guid.Parse(device.BT_GUID), par);
                    if (!(adapterDevice is null)) await adapter.DisconnectDeviceAsync(adapterDevice);
                } catch (Exception e)
                {
                    Console.WriteLine($"Checking if {device.UserLabel} is reachable failed:\n"+e.ToString());
                }
            }
            return adapterDevice != null ? adapterDevice.Rssi : int.MinValue;
        }

        public bool IsEnabled()
        {
            IBluetoothLE ble = CrossBluetoothLE.Current;

            return ble.State == BluetoothState.On || ble.State == BluetoothState.TurningOn;
        }

        public static double RssiToMeter(double rssi, double measuredPower, double environmentalFactor = Constants.RssiEnvironmentalDefault)
        {
            return Math.Pow(10, (measuredPower - rssi) / (10 * environmentalFactor));
        }
    }
}
