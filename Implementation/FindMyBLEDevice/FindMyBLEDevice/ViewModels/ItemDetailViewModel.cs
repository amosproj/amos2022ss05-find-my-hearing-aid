using FindMyBLEDevice.Models;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private int _currentRssi;
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
