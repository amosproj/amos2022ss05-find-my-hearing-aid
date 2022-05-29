using FindMyBLEDevice.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    [QueryProperty(nameof(DeviceId), nameof(DeviceId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private int deviceId;
        private BTDevice device;

        public int DeviceId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
                LoadItemId(value);
            }
        }

        public BTDevice Device
        {
            get
            {
                return device;
            }
        }

        public async Task LoadItemId(int deviceId)
        {
            try
            {
                device = await App.DevicesStore.GetDevice(deviceId);
                OnPropertyChanged("Device");
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
