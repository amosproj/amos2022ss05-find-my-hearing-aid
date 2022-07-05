using System;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private Models.BTDevice device;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            device = new Models.BTDevice();
        }

        public void OnAppearing()
        {
            device = App.DevicesStore.SelectedDevice;
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(device.UserLabel);
        }

        public string BTGUID
        {
            get => device.BT_GUID;
        }

        public string UserLabel
        {
            get => device.UserLabel;
            set {
                if (device.UserLabel != value)
                {
                    device.UserLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        public string AdvertisedName
        {
            get => device.AdvertisedName;
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            if(String.IsNullOrWhiteSpace(device.UserLabel))
            {
                device.UserLabel = device.AdvertisedName;
            }

            App.DevicesStore.SelectedDevice = await App.DevicesStore.AddDevice(device);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
