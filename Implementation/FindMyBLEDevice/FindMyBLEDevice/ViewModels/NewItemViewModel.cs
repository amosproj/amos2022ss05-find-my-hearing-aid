﻿// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Geolocation;
using System;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private readonly INavigator navigator;
        private readonly IDevicesStore devicesStore;
        private readonly IGeolocation geolocation;

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public BTDevice Device => devicesStore.SelectedDevice;

        private string _userLabel;
        public string UserLabel
        {
            get => _userLabel;
            set => SetProperty(ref _userLabel, value);
        }

        public NewItemViewModel(INavigator navigator, IDevicesStore devicesStore, IGeolocation geolocation)
        {
            this.navigator = navigator;
            this.devicesStore = devicesStore;
            this.geolocation = geolocation;

            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            devicesStore.SelectedDevice = null;
            await navigator.GoToAsync("..");
        }

        private async void OnSave()
        {
            if(String.IsNullOrWhiteSpace(UserLabel))
            {
                Device.UserLabel = Device.AdvertisedName;
            } else
            {
                Device.UserLabel = UserLabel;
            }

            var location = await geolocation.GetCurrentLocation();
            Device.LastGPSLongitude = location.Longitude;
            Device.LastGPSLatitude = location.Latitude;
            Device.LastGPSTimestamp = DateTime.Now;

            devicesStore.SelectedDevice = await devicesStore.AddDevice(Device);

            // This will pop the current page off the navigation stack
            await navigator.GoToAsync("..");
        }

        public void OnAppearing()
        {
            UserLabel = null;
        }
    }
}
