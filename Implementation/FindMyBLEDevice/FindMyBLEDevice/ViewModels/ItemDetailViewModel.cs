// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private int _currentRssi;
        public Command RenameButtonTapped { get; }
        public Command StrengthButtonTapped { get; }
        public Command MapButtonTapped { get; }
        public Command DeleteButtonTapped { get; }

        public string UserLabel { get; set; }

        public ItemDetailViewModel()
        {
            RenameButtonTapped = new Command(
                   async () => await RenameDevice());
            StrengthButtonTapped = new Command(
                   async () => await RedirectTo(nameof(StrengthPage)));
            MapButtonTapped = new Command(
                async () => await RedirectTo(nameof(MapPage)));
            DeleteButtonTapped = new Command(
                async () => await DeleteDevice());

            UserLabel = Device.UserLabel;
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

        public bool UserLabelEdited
        {
            get => UserLabel != Device.UserLabel;
        }

        public void UserLabel_TextChanged()
        {
            OnPropertyChanged("UserLabelEdited");
        }

        async Task RenameDevice()
        {
            
            // Show confirmation dialog
            bool answer = await Application.Current.MainPage.DisplayAlert("Rename device", String.Format("Are you sure you want to rename this device to '{0}'?", UserLabel), "Yes", "Cancel");

            if (!answer)
            {
                return;
            }

            Device.UserLabel = UserLabel;
            await App.DevicesStore.UpdateDevice(Device);
            OnPropertyChanged("Device");
            OnPropertyChanged("UserLabelEdited");
        }

        async Task DeleteDevice()
        {

            // Show confirmation dialog
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete device", "Are you sure you want to delete this device?", "Yes", "Cancel");

            if (!answer)
            {
                return;
            }

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
