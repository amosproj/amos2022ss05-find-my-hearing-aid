// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private readonly INavigator navigator;
        private readonly IBluetooth bluetooth;
        private readonly IDevicesStore devicesStore;

        private int _currentRssi;
        public Command StrengthButtonTapped { get; }
        public Command MapButtonTapped { get; }

        public ItemDetailViewModel(INavigator navigator, IBluetooth bluetooth, IDevicesStore devices)
        {
            this.navigator = navigator;
            this.bluetooth = bluetooth;
            this.devicesStore = devices;

            StrengthButtonTapped = new Command(
                   async () => await RedirectTo(nameof(StrengthPage)));
            MapButtonTapped = new Command(
                async () => await RedirectTo(nameof(MapPage)));
        }
        async Task RedirectTo(string page)
        {
            bluetooth.StopRssiPolling();
            await navigator.GoToAsync(page);
        }
        public int CurrentRssi
        {
            get => _currentRssi;
            set => SetProperty(ref _currentRssi, value);
        }

        public BTDevice Device
        {
            get => devicesStore.SelectedDevice;
        }

        public void OnAppearing()
        {
            bluetooth.StartRssiPolling(Device.BT_GUID, (int v, int txPower) => {
                CurrentRssi = v;
            });

        }
        public void OnDisappearing()
        {
            bluetooth.StopRssiPolling();
        }
    }
}
