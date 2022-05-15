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
        private string deviceId;
        private string name;
        private string bt_id;
        public string Id { get; set; }

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

        public string DeviceID
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

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Text;
                BT_id = item.Description;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
