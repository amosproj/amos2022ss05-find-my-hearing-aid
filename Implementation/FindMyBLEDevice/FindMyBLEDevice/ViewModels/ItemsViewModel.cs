// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using FindMyBLEDevice.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Location;
using System.Threading;
using FindMyBLEDevice.Services;

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
        public Command<BTDevice> SavedDeviceSettingsTapped { get; }
        public Command OpenInfoPageCommand { get; }
        public Command GoBack { get; }

        private ObservableCollection<BTDevice> _savedDevices;
        public ObservableCollection<BTDevice> SavedDevices
        {
            get => _savedDevices;
            set => SetProperty(ref _savedDevices, value);
        }

        private ObservableCollection<BTDevice> _availableDevices;
        public ObservableCollection<BTDevice> AvailableDevices
        {
            get => _availableDevices;
            set => SetProperty(ref _availableDevices, value);
        }

        public bool IsBusyAndNothingFound
        {
            get => IsBusy && AvailableDevices.Count == 0;
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
                async (BTDevice device) => await SelectAndRedirectTo(device, ".."));
            SavedDeviceSettingsTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, navigator.DeviceDetailPage));
            AvailableDeviceTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, navigator.NewDevicePage));
            OpenInfoPageCommand = new Command(
                async () => await navigator.GoToAsync(navigator.InfoPage));
            GoBack = new Command(
                async () => await navigator.GoToAsync(".."));
            
            PropertyChanged += IsBusyChanged;

        }

        private void IsBusyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IsBusy))
            {
                OnPropertyChanged(nameof(IsBusyAndNothingFound));
            }
        }

        private void AvailableDevicesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsBusyAndNothingFound));
        }

        private async Task SelectAndRedirectTo(BTDevice device, string page, bool newStack = false)
        {
            if (device == null)
                return;

            devicesStore.SelectedDevice = device;
            await navigator.GoToAsync(page, newStack);
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
            AvailableDevices.CollectionChanged += AvailableDevicesChanged;
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

        public async void OnAppearing()
        {
            await CheckBluetoothAndLocation.Check();

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
    }
}
