// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private int _currentRssi;
        public Command GoBack { get; }
        public Command RenameButtonTapped { get; }

        public Command DeleteButtonTapped { get; }

        public string UserLabel { get; set; }

        public ItemDetailViewModel()
        {
            GoBack = new Command(async () => await Shell.Current.GoToAsync(".."));
            RenameButtonTapped = new Command(
                   async () => await RenameDevice());
            DeleteButtonTapped = new Command(
                async () => await DeleteDevice());

            UserLabel = Device.UserLabel;
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
            Device.UserLabel = UserLabel;
            await App.DevicesStore.UpdateDevice(Device);
            OnPropertyChanged("Device");
            OnPropertyChanged("UserLabelEdited");
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
