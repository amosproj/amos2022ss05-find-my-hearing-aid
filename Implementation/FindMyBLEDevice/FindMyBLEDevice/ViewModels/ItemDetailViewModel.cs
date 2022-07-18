// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using System;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private readonly INavigator navigator;
        private readonly IBluetooth bluetooth;
        private readonly IDevicesStore devicesStore;
        private readonly string _message = "On this screen you can rename your device,"
                          + "find additional information and delete the device from the 'Saved Devices' section of the previous page.";


        public Command StrengthButtonTapped { get; }
        public Command MapButtonTapped { get; }
        public Command RenameButtonTapped { get; }
        public Command DeleteButtonTapped { get; }
        public Command ShowInfoPage { get; }
        public Command GoBack { get; }

        public BTDevice Device => devicesStore.SelectedDevice;

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
            set
            {
                if (_userLabel != value && value.Length <= Constants.UserLabelMaxLength)
                {
                    _userLabel = value;
                }
                OnPropertyChanged(nameof(UserLabel));
            }
        }

        public bool UserLabelEdited
        {
            get => UserLabel != Device.UserLabel;
        }

        public ItemDetailViewModel(INavigator navigator, IBluetooth bluetooth, IDevicesStore devicesStore)
        {
            this.navigator = navigator;
            this.bluetooth = bluetooth;
            this.devicesStore = devicesStore;

            StrengthButtonTapped = new Command(
                async () => await navigator.GoToAsync(navigator.StrengthPage, true));
            MapButtonTapped = new Command(
                async () => await navigator.GoToAsync(navigator.MapPage, true));
            RenameButtonTapped = new Command(
                async () => await RenameDevice());
            DeleteButtonTapped = new Command(
                async () => await DeleteDevice());
            GoBack = new Command(
                async () => await navigator.GoToAsync(".."));
            ShowInfoPage = new Command(
                async () => await App.Current.MainPage.DisplayAlert("Information", _message, "Ok"));

            PropertyChanged += DeviceOrUserLabelChanged;
            UserLabel = Device.UserLabel;
        }

        private void DeviceOrUserLabelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Device) || e.PropertyName == nameof(UserLabel))
                OnPropertyChanged(nameof(UserLabelEdited));
        }

        private async Task RenameDevice()
        {

            // Check for UserLabel constraints
            if ((await devicesStore.GetAllDevices()).Any(d => d.UserLabel == UserLabel))
            {
                await App.Current.MainPage.DisplayAlert("Label already taken", $"The label '{UserLabel}' is already taken by another device. Please choose another one.", "Ok");
                return;
            } 
            
            // Show confirmation dialog
            bool answer = await Application.Current.MainPage.DisplayAlert(
                "Rename device", 
                String.Format("Are you sure you want to rename this device to '{0}'?", UserLabel), 
                "Yes", "Cancel");

            if (!answer)
            {
                UserLabel = Device.UserLabel;
                OnPropertyChanged("UserLabel");
                OnPropertyChanged("UserLabelEdited");
                return;
            }

            Device.UserLabel = UserLabel;
            await devicesStore.UpdateDevice(Device);
            OnPropertyChanged(nameof(Device));
        }

        private async Task DeleteDevice()
        {
            // Show confirmation dialog
            bool answer = await Application.Current.MainPage.DisplayAlert(
                "Delete device", 
                "Are you sure you want to delete this device?", 
                "Yes", "Cancel");
            if (!answer)
            {
                return;
            }

            var id = Device.ID;
            devicesStore.SelectedDevice = null;
            try
            {
                await devicesStore.DeleteDevice(id);
            } catch (Exception e)
            {
                Console.WriteLine($"[DevicesView] Deleting device failed: {e.StackTrace}");
            }

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
