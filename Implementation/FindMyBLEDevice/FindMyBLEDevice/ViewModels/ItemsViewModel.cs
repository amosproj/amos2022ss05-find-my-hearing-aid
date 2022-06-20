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

namespace FindMyBLEDevice.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {

        private ObservableCollection<BTDevice> availableDevices;
        public ObservableCollection<BTDevice> SavedDevices { get; }

        public Command LoadSavedDevicesCommand { get; }
        public Command LoadAvailableDevicesCommand { get; }
        public Command SearchAvailableDevicesCommand { get; }
        public Command<BTDevice> SavedDeviceTapped { get; }
        public Command<BTDevice> AvailableDeviceTapped { get; }
        public Command<BTDevice> StrengthButtonTapped { get; }
        public Command<BTDevice> MapButtonTapped { get; }

        public ObservableCollection<BTDevice> AvailableDevices {
            get { 
                return availableDevices; 
            }
            set
            {
                List<BTDevice> devices = value.ToList();
                availableDevices = new ObservableCollection<BTDevice>(devices);                
                OnPropertyChanged(nameof(AvailableDevices));
            }
        }

        public ItemsViewModel()
        {
            Title = "Devices";

            SavedDevices = new ObservableCollection<BTDevice>();
            LoadSavedDevicesCommand = new Command(async () => await ExecuteLoadSavedDevicesCommand());

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

            List<BTDevice> savedDevices = await App.DevicesStore.GetAllDevices();
            List<BTDevice> available = AvailableDevices.ToList();
            available.RemoveAll(availableDevice => savedDevices.Exists(saved => (saved.BT_GUID.CompareTo(availableDevice.BT_GUID)) == 0));
            AvailableDevices = new ObservableCollection<BTDevice>(available);
        }

        async Task SelectAndRedirectTo(BTDevice device, string page)
        {
            if (device == null)
                return;

            await App.Bluetooth.StopSearch();

            App.DevicesStore.SelectedDevice = device;
            await Shell.Current.GoToAsync(page);
        }

        private async Task ExecuteSearchAvailableDevicesCommand()
        {
            List<BTDevice> savedDevices = await App.DevicesStore.GetAllDevices();
            await App.Location.CheckAndRequestLocationPermission();
            await App.Bluetooth.Search(20000, AvailableDevices, 
                found => !savedDevices.Exists(saved => saved.BT_GUID.Equals(found.BT_GUID)));
        }

    }
}
