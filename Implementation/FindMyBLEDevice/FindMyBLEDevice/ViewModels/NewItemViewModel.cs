using System;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    [QueryProperty(nameof(BTGUID), nameof(BTGUID))]
    [QueryProperty(nameof(AdvertisedName), nameof(AdvertisedName))]
    public class NewItemViewModel : BaseViewModel
    {
        private string btGuid;
        private string advertisedName;
        private string userLabel;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(userLabel);
        }

        public string BTGUID
        {
            get => btGuid;
            set => SetProperty(ref btGuid, value);
        }

        public string UserLabel
        {
            get => userLabel;
            set => SetProperty(ref userLabel, value);
        }

        public string AdvertisedName
        {
            get => advertisedName;
            set => SetProperty(ref advertisedName, value);
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

            await App.DevicesStore.AddDevice(btGuid, advertisedName, userLabel);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
