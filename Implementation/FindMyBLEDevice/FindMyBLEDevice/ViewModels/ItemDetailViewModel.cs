using FindMyBLEDevice.Models;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    [QueryProperty(nameof(DeviceId), nameof(DeviceId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private int deviceId;
        private BTDevice device;

        private int _currentRssi;
        public int CurrentRssi
        {
            get
            {
                return _currentRssi;
            }
            set
            {
                _currentRssi = value;
                OnPropertyChanged(nameof(CurrentRssi));
            }
        }

        public int DeviceId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
            }
        }

        public BTDevice Device
        {
            get
            {
                return device;
            }
        }

        public async void OnAppearing()
        {

            device = await App.DevicesStore.GetDevice(deviceId);
            OnPropertyChanged("Device");
            
            await App.Bluetooth.StartRssiPolling(device.BT_GUID, (int v) => {
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
