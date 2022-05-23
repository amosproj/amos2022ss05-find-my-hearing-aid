using FindMyBLEDevice.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class BluetoothViewModel : BaseViewModel
    {
        public ObservableCollection<AvailableBTDevice> AvailableDevices { get; }
        public Command LoadDevicesCommand { get; }
        public Command SearchDevicesCommand { get; }
        public BluetoothViewModel()
        {
            Title = "Available BT Devices";
            AvailableDevices = new ObservableCollection<AvailableBTDevice>();
            LoadDevicesCommand = new Command(async () => await ExecuteLoadDevicesCommand());
            SearchDevicesCommand = new Command(async () => await ExecuteSearchDevicesCommand());
        }

        private async Task ExecuteLoadDevicesCommand()
        {
            AvailableDevices.Clear();
            List<AvailableBTDevice> dev = App.Bluetooth.GetAvailableDevices();
            dev.ForEach(AvailableDevices.Add);
        }

        private async Task ExecuteSearchDevicesCommand()
        {
            await App.Bluetooth.search(5000);
        }

    }
}
