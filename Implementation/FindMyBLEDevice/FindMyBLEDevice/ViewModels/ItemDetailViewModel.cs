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
        public Command DeleteButtonTapped { get; }

        public ItemDetailViewModel()
        {
            StrengthButtonTapped = new Command(
                   async () => await RedirectTo(nameof(StrengthPage)));
            MapButtonTapped = new Command(
                async () => await RedirectTo(nameof(MapPage)));
            DeleteButtonTapped = new Command(
                async () => await DeleteDevice());
        }
        async Task RedirectTo(string page)
        {
            App.Bluetooth.StopRssiPolling();
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

        async Task DeleteDevice()
        {
            App.Bluetooth.StopRssiPolling();

            int id = Device.ID;
            App.DevicesStore.SelectedDevice = null;
            await App.DevicesStore.DeleteDevice(id);

            // Go back to devices page
            await Shell.Current.GoToAsync("..");
        }

        public void OnAppearing()
        {
            App.Bluetooth.StartRssiPolling(Device.BT_GUID, (int v, int txPower) => {
                CurrentRssi = v;
            });
        }
        public void OnDisappearing()
        {
            App.Bluetooth.StopRssiPolling();
        }
    }
}
