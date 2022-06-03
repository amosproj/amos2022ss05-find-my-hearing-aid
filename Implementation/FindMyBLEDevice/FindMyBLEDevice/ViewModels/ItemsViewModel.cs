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
using System.Windows.Input;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private BTDevice _selectedSavedDevice;

        private AvailableBTDevice _selectedAvailableDevice;


        private ObservableCollection<AvailableBTDevice> availableDevices;
        public ObservableCollection<BTDevice> SavedDevices { get; }

        public Command LoadSavedDevicesCommand { get; }
        public Command<BTDevice> SavedDeviceTapped { get; }
        public Command<BTDevice> SavedDeviceButtonPressed { get; }
        public Command<AvailableBTDevice> AvailableDeviceTapped { get; }
        public Command LoadAvailableDevicesCommand { get; }
        public Command SearchAvailableDevicesCommand { get; }

        public ObservableCollection<AvailableBTDevice> AvailableDevices {
            get { 
                return availableDevices; 
            }
            set
            {
                List<AvailableBTDevice> devices = value.ToList();
                devices.Sort((x, y) => y.Rssi.CompareTo(x.Rssi));
                availableDevices = new ObservableCollection<AvailableBTDevice>(devices);                
                OnPropertyChanged("AvailableDevices");
            }
        }

        public ItemsViewModel()
        {
            Title = "Devices";

            SavedDevices = new ObservableCollection<BTDevice>();
            LoadSavedDevicesCommand = new Command(async () => await ExecuteLoadSavedDevicesCommand());

            SavedDeviceTapped = new Command<BTDevice>(OnSavedDeviceSelected);
            AvailableDeviceTapped = new Command<AvailableBTDevice>(OnAvailableDeviceSelected);

            AvailableDevices = new ObservableCollection<AvailableBTDevice>();
            //LoadAvailableDevicesCommand = new Command(() => ExecuteLoadAvailableDevicesCommand()); // we have to find an alternative
            SearchAvailableDevicesCommand = new Command(() => ExecuteSearchAvailableDevicesCommand());
            SavedDeviceButtonPressed = new Command<BTDevice>(OnSavedButtonPressed);
        }

        async Task ExecuteLoadSavedDevicesCommand()
        {
            IsBusy = true;

            try
            {
                SavedDevices.Clear();
                var devices = await App.DevicesStore.GetAllDevices();
                foreach (var device in devices)
                {
                    SavedDevices.Add(device);
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
            SelectedSavedDevice = null;

            List<BTDevice> savedDevices = await App.DevicesStore.GetAllDevices();
            List<AvailableBTDevice> available = AvailableDevices.ToList();
            available.RemoveAll(availableDevice => savedDevices.Exists(saved => saved.BT_GUID == availableDevice.Id.ToString()));
            AvailableDevices = new ObservableCollection<AvailableBTDevice>(available);
        }

        public BTDevice SelectedSavedDevice
        {
            get => _selectedSavedDevice;
            set
            {
                SetProperty(ref _selectedSavedDevice, value);
                OnSavedDeviceSelected(value);
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

        async void OnSavedDeviceSelected(BTDevice device)
        {
            if (device == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.DeviceId)}={device.Id}");
        }
        async void OnSavedButtonPressed(BTDevice device)
        {
            if (device == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(StrengthPage)}?{nameof(StrengthViewModel.BT_id)}={device.Id}");
        }

        async void OnAvailableDeviceSelected(AvailableBTDevice device)
        {
            if (device == null)
                return;

            await App.Bluetooth.StopSearch();

            // Alternative: open add item page instead of signal strength page
            // This will push the NewItemPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.BTGUID)}={device.Id}&{nameof(NewItemViewModel.AdvertisedName)}={device.Name}");
        }

        private async Task ExecuteSearchAvailableDevicesCommand()
        {
            List<BTDevice> savedDevices = await App.DevicesStore.GetAllDevices();
            await App.Bluetooth.Search(20000, AvailableDevices, found => savedDevices.Exists(saved => saved.BT_GUID.Equals(found.Id.ToString())));
        }



        public ICommand RedirectToStrengthPage
        {
            get
            {
                return new Command(async (e) =>
                {
                    var selectedDevice = (e as Models.AvailableBTDevice);
                    await App.Bluetooth.StopSearch();
                    await Shell.Current.GoToAsync($"{nameof(StrengthPage)}?{nameof(StrengthViewModel.BT_id)}={selectedDevice.Id}");
                });
            }
        }
    }
}
