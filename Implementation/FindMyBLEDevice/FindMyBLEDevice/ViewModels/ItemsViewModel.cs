// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using FindMyBLEDevice.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Location;
using System.Threading;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private readonly INavigator navigator;
        private readonly IDevicesStore devicesStore;
        private readonly IBluetooth bluetooth;
        private readonly ILocation location;

        private CancellationTokenSource isBusyCancel;

        public Command SearchAvailableDevicesCommand { get; }
        public Command<BTDevice> SavedDeviceTapped { get; }
        public Command<BTDevice> AvailableDeviceTapped { get; }
        public Command<BTDevice> StrengthButtonTapped { get; }
        public Command<BTDevice> MapButtonTapped { get; }

        private ObservableCollection<BTDevice> savedDevices, availableDevices;
        public ObservableCollection<BTDevice> SavedDevices
        {
            get => savedDevices;
            set => SetProperty(ref savedDevices, value);
        }
        public ObservableCollection<BTDevice> AvailableDevices
        {
            get => availableDevices;
            set => SetProperty(ref availableDevices, value);
        }

        public ItemsViewModel(INavigator navigator, IDevicesStore devicesStore, IBluetooth bluetooth, ILocation location)
        {
            Title = "Devices";

            this.navigator = navigator;
            this.devicesStore = devicesStore;
            this.bluetooth = bluetooth;
            this.location = location;

            SavedDevices = new ObservableCollection<BTDevice>();
            AvailableDevices = new ObservableCollection<BTDevice>();

            SearchAvailableDevicesCommand = new Command(
                async () => await ExecuteSearchAvailableDevicesCommand());
            SavedDeviceTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, navigator.DeviceDetailPage));
            AvailableDeviceTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, navigator.NewDevicePage));
            StrengthButtonTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, navigator.StrengthPage));
            MapButtonTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, navigator.MapPage));
        }

        private async Task SelectAndRedirectTo(BTDevice device, string page)
        {
            if (device == null)
                return;

            devicesStore.SelectedDevice = device;
            await navigator.GoToAsync(page);
        }

        public void OnAppearing()
        {
            devicesStore.DevicesChanged += OnDevicesChanged;
            OnDevicesChanged(null, null);
            AvailableDevices = new ObservableCollection<BTDevice>();
            bluetooth.DeviceDiscovered += AvailableDeviceDiscovered;
        }

        public async void OnDisappearing()
        {
            await bluetooth.StopSearch();
            isBusyCancel?.Cancel();

            devicesStore.DevicesChanged -= OnDevicesChanged;
            bluetooth.DeviceDiscovered -= AvailableDeviceDiscovered;
        }

        private async void OnDevicesChanged(object sender, EventArgs e)
        {
            try
            {
                SavedDevices = new ObservableCollection<BTDevice>(await devicesStore.GetAllDevices());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void AvailableDeviceDiscovered(object sender, BTDevice device)
        {
            if (AvailableDevices.ToList<BTDevice>().Exists(other => other.BT_GUID == device.BT_GUID))
            {
                return;
            }

            if (SavedDevices.ToList<BTDevice>().Exists(saved => saved.BT_GUID.Equals(device.BT_GUID)))
            {
                return;
            }

            AvailableDevices.Add(device);
        }

        private async Task ExecuteSearchAvailableDevicesCommand()
        {
            await bluetooth.StopSearch();
            isBusyCancel?.Cancel();
            isBusyCancel = new CancellationTokenSource();
            CancellationToken cancellationToken = isBusyCancel.Token;
            IsBusy = true;

            await location.CheckAndRequestLocationPermission();
            AvailableDevices = new ObservableCollection<BTDevice>();
            await bluetooth.StartSearch(Constants.DiscoverSearchDuration);

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(Constants.DiscoverSearchDuration, cancellationToken);
                }
                finally
                {
                    IsBusy = false;
                }
            }, cancellationToken);
        }
    }
}
