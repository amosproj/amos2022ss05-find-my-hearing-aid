// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private readonly INavigator navigator;
        private readonly IBluetooth bluetooth;
        private readonly IDevicesStore devicesStore;

        public Command StrengthButtonTapped { get; }
        public Command MapButtonTapped { get; }
        public Command RenameButtonTapped { get; }
        public Command DeleteButtonTapped { get; }

        public BTDevice Device
        {
            get => devicesStore.SelectedDevice;
        }

        private int _currentRssi;
        public int CurrentRssi
        {
            get => _currentRssi;
            set => SetProperty(ref _currentRssi, value);
        }

        private string _userLabel;
        public string UserLabel
        {
            get => _userLabel;
            set => SetProperty(ref _userLabel, value);
        }

        public bool UserLabelEdited
        {
            get => UserLabel != Device.UserLabel;
        }

        public ItemDetailViewModel(INavigator navigator, IBluetooth bluetooth, IDevicesStore devices)
        {
            this.navigator = navigator;
            this.bluetooth = bluetooth;
            this.devicesStore = devices;

            StrengthButtonTapped = new Command(
                async () => await navigator.GoToAsync(navigator.StrengthPage));
            MapButtonTapped = new Command(
                async () => await navigator.GoToAsync(navigator.MapPage));
            RenameButtonTapped = new Command(
                async () => await RenameDevice());
            DeleteButtonTapped = new Command(
                async () => await DeleteDevice());

            PropertyChanged += DeviceOrUserLabelChanged;
            UserLabel = Device.UserLabel;
        }

        private void DeviceOrUserLabelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Device) || e.PropertyName == nameof(UserLabel))
                OnPropertyChanged(nameof(UserLabelEdited));
        }

        async Task RenameDevice()
        {
            Device.UserLabel = UserLabel;
            await devicesStore.UpdateDevice(Device);
            OnPropertyChanged(nameof(Device));
        }

        async Task DeleteDevice()
        {
            await devicesStore.DeleteDevice(Device.ID);
            devicesStore.SelectedDevice = null;

            // Go back to devices page
            await navigator.GoToAsync("..");
        }

        public void OnAppearing()
        {
            bluetooth.StartRssiPolling(Device.BT_GUID, (int rssi, int txPower) =>
            {
                CurrentRssi = rssi;
            });
        }

        public void OnDisappearing()
        {
            bluetooth.StopRssiPolling();
        }
    }
}
