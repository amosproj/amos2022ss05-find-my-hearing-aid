// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private int _currentRssi;
        public Command StrengthButtonTapped { get; }
        public Command MapButtonTapped { get; }

        public ItemDetailViewModel()
        {
            StrengthButtonTapped = new Command(
                   async () => await SelectAndRedirectTo(nameof(StrengthPage)));
            MapButtonTapped = new Command(
                async () => await SelectAndRedirectTo(nameof(MapPage)));
        }
        async Task SelectAndRedirectTo(string page)
        {
            Console.WriteLine("Hello from my side");

            App.Bluetooth.StopRssiPolling();

            Console.WriteLine("../" + page);
            await Shell.Current.GoToAsync(page);
        }
        public int CurrentRssi
        {
            get => _currentRssi;
            set => SetProperty(ref _currentRssi, value);
        }

        public BTDevice Device
        {
            get => App.DevicesStore.SelectedDevice;
        }

        public async void OnAppearing()
        {
            await App.Bluetooth.StartRssiPolling(Device.BT_GUID, (int v) => {
                CurrentRssi = v;
                return 0;
            });

        }
        public void OnDisappearing()
        {
            App.Bluetooth.StopRssiPolling();
        }
    }
}
