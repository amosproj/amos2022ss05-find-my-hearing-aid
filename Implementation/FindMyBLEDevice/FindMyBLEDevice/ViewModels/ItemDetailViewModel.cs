using FindMyBLEDevice.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    [QueryProperty(nameof(DeviceID), nameof(DeviceID))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private int deviceId;
        private string name;
        private string bt_id;
        public int Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string BT_id
        {
            get => bt_id;
            set => SetProperty(ref bt_id, value);
        }

        public int DeviceID
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

        public async void LoadItemId(int deviceId)
        {
            try
            {
                BTDevice device = await App.DevicesStore.GetDevice(deviceId);
                Id = device.Id;
                Name = device.Name;
                BT_id = device.BT_id;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
