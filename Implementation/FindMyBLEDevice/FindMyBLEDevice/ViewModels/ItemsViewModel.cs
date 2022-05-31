// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;


namespace FindMyBLEDevice.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private BTDevice _selectedPairedDevice;

        private AvailableBTDevice _selectedAvailableDevice;

        public ObservableCollection<BTDevice> PairedDevices { get; }
        public Command LoadPairedDevicesCommand { get; }
        public Command AddPairedDeviceCommand { get; }
        public Command<BTDevice> PairedDeviceTapped { get; }
        public Command<AvailableBTDevice> AvailableDeviceTapped { get; }

        private ObservableCollection<AvailableBTDevice> _AvailableDevices;
        public ObservableCollection<AvailableBTDevice> AvailableDevices {
            get { 
                return _AvailableDevices; 
            }
            set
            {
                List<AvailableBTDevice> devices = value.ToList();
                devices.Sort((x, y) => y.Rssi.CompareTo(x.Rssi));
                _AvailableDevices = new ObservableCollection<AvailableBTDevice>(devices);                
                OnPropertyChanged("AvailableDevices");
            }
        }

        public Command LoadAvailableDevicesCommand { get; }
        public Command SearchAvailableDevicesCommand { get; }

        public ItemsViewModel()
        {
            Title = "Devices";

            PairedDevices = new ObservableCollection<BTDevice>();
            LoadPairedDevicesCommand = new Command(async () => await ExecuteLoadPairedDevicesCommand());

            PairedDeviceTapped = new Command<BTDevice>(OnPairedDeviceSelected);
            AvailableDeviceTapped = new Command<AvailableBTDevice>(OnAvailableDeviceSelected);

            AddPairedDeviceCommand = new Command(OnAddPairedDevice);

            AvailableDevices = new ObservableCollection<AvailableBTDevice>();
            //LoadAvailableDevicesCommand = new Command(() => ExecuteLoadAvailableDevicesCommand()); // we have to find an alternative
            SearchAvailableDevicesCommand = new Command(() => ExecuteSearchAvailableDevicesCommand());
        }

        async Task ExecuteLoadPairedDevicesCommand()
        {
            IsBusy = true;

            try
            {
                PairedDevices.Clear();
                var devices = await App.DevicesStore.GetAllDevices();
                foreach (var device in devices)
                {
                    PairedDevices.Add(device);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            SelectedPairedDevice = null;

            List<BTDevice> savedDevices = await App.DevicesStore.GetAllDevices();
            List<AvailableBTDevice> available = AvailableDevices.ToList();
            available.RemoveAll(availableDevice => savedDevices.Exists(saved => saved.BT_GUID == availableDevice.Id.ToString()));
            AvailableDevices = new ObservableCollection<AvailableBTDevice>(available);
        }

        public BTDevice SelectedPairedDevice
        {
            get => _selectedPairedDevice;
            set
            {
                SetProperty(ref _selectedPairedDevice, value);
                OnPairedDeviceSelected(value);
            }
        }

        public AvailableBTDevice SelectedAvailableDevice
        {
            get => _selectedAvailableDevice;
            set
            {
                SetProperty(ref _selectedAvailableDevice, value);
                OnAvailableDeviceSelected(value);
            }
        }

        private async void OnAddPairedDevice(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnPairedDeviceSelected(BTDevice device)
        {
            if (device == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.DeviceId)}={device.Id}");
        }

        async void OnAvailableDeviceSelected(AvailableBTDevice device)
        {
            if (device == null)
                return;

            await App.Bluetooth.StopSearch();

            // This will replace the navigation stack with StrengthPage:
            // TODO: find a way to just push the site onto the navigation stack
            await Shell.Current.GoToAsync($"../StrengthPage?bt_id={device.Id}");

            // Alternative: open add item page instead of signal strength page
            // This will push the NewItemPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.BTGUID)}={device.Id}&{nameof(NewItemViewModel.AdvertisedName)}={device.Name}");
        }

        private async Task ExecuteSearchAvailableDevicesCommand()
        {
            List<BTDevice> savedDevices = await App.DevicesStore.GetAllDevices();
            await App.Bluetooth.Search(20000, AvailableDevices, found => savedDevices.Exists(saved => saved.BT_GUID.Equals(found.Id.ToString())));
        }

    }
}
