// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Views;

namespace FindMyBLEDevice.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string _selectedDeviceString;

        public AboutViewModel()
        {
            Title = "Home";
            _selectedDeviceString = "Please select a device first.";
            OpenMapPageCommand = new Command(async () => await Shell.Current.GoToAsync($"//{nameof(MapPage)}"));
            OpenStrengthPageCommand = new Command(async () => await Shell.Current.GoToAsync($"//{nameof(StrengthPage)}"));
            OpenInfoPageCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(InfoPage)}"));
            SelectDevice = new Command(async () => await Shell.Current.GoToAsync($"{nameof(ItemsPage)}"));
        }

        public ICommand OpenMapPageCommand { get; }

        public ICommand OpenStrengthPageCommand { get; }

        public ICommand OpenInfoPageCommand { get; }
        public Command SelectDevice { get; }
        public BTDevice Device { get => App.DevicesStore.SelectedDevice; }

        public string SelectedDeviceString
        {
            get => _selectedDeviceString;
            set => SetProperty(ref _selectedDeviceString, value);
        }

        public async void OnAppearing()
        {
            if (!(App.DevicesStore.SelectedDevice is null))
            {
                SelectedDeviceString = "" + App.DevicesStore.SelectedDevice.UserLabel + "\nClick to select another device.";
            }
        }
        public void OnDisappearing()
        {
            // comment to make linter happy, method will be used in the future
        }
    }
}
