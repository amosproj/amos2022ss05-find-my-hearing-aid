// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

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
using FindMyBLEDevice.Services;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {

        private ObservableCollection<BTDevice> savedDevices, availableDevices;
        public ObservableCollection<BTDevice> SavedDevices 
        { 
            get => savedDevices; 
            set
            {
                savedDevices = value;
                OnPropertyChanged(nameof(SavedDevices));
            }
        }
        public ObservableCollection<BTDevice> AvailableDevices {
            get { 
                return availableDevices; 
            }
            set
            {
                availableDevices = value;
                OnPropertyChanged(nameof(AvailableDevices));
            }
        }

        public Command LoadSavedDevicesCommand { get; }
        public Command LoadAvailableDevicesCommand { get; }
        public Command SearchAvailableDevicesCommand { get; }
        public Command<BTDevice> SavedDeviceTapped { get; }
        public Command<BTDevice> AvailableDeviceTapped { get; }
        public Command<BTDevice> StrengthButtonTapped { get; }
        public Command<BTDevice> MapButtonTapped { get; }

        public ItemsViewModel()
        {
            Title = "Devices";

            SavedDevices = new ObservableCollection<BTDevice>();
            AvailableDevices = new ObservableCollection<BTDevice>();
            //LoadAvailableDevicesCommand = new Command(() => ExecuteLoadAvailableDevicesCommand()); // we have to find an alternative
#pragma warning disable CS4014
            SearchAvailableDevicesCommand = new Command(() => ExecuteSearchAvailableDevicesCommand());
#pragma warning restore CS4014
            
            SavedDeviceTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, nameof(ItemDetailPage)));
            AvailableDeviceTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, nameof(NewItemPage)));
            StrengthButtonTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, nameof(StrengthPage)));
            MapButtonTapped = new Command<BTDevice>(
                async (BTDevice device) => await SelectAndRedirectTo(device, nameof(MapPage)));
        }

        public async void OnAppearing()
        {
            IsBusy = true;;

            App.DevicesStore.DevicesChanged += OnDevicesChanged;
            OnDevicesChanged(null, null);

            AvailableDevices = new ObservableCollection<BTDevice>();

            IsBusy = false;
        }

        private async void OnDevicesChanged(object sender, EventArgs e)
        {
            try
            {
                SavedDevices = new ObservableCollection<BTDevice>(await App.DevicesStore.GetAllDevices());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async void OnDisappearing()
        {
            await App.Bluetooth.StopSearch();

            App.DevicesStore.DevicesChanged -= OnDevicesChanged;
        }

        async Task SelectAndRedirectTo(BTDevice device, string page)
        {
            if (device == null)
                return;

            App.DevicesStore.SelectedDevice = device;
            await Shell.Current.GoToAsync(page);
        }

        private async Task ExecuteSearchAvailableDevicesCommand()
        {
            List<BTDevice> savedDevicesList = SavedDevices.ToList();
            await App.Location.CheckAndRequestLocationPermission();
            await App.Bluetooth.Search(20000, AvailableDevices, 
                found => !savedDevicesList.Exists(saved => saved.BT_GUID.Equals(found.BT_GUID)));
        }

    }
}
