using FindMyBLEDevice.Models;
using FindMyBLEDevice.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private BTDevice _selectedPairedDevice;

        public ObservableCollection<BTDevice> PairedDevices { get; }
        public Command LoadPairedDevicesCommand { get; }
        public Command AddPairedDeviceCommand { get; }
        public Command<BTDevice> PairedDeviceTapped { get; }
        public ObservableCollection<AvailableBTDevice> AvailableDevices { get; }
        public Command LoadAvailableDevicesCommand { get; }
        public Command SearchAvailableDevicesCommand { get; }

        public ItemsViewModel()
        {
            Title = "Devices";
            PairedDevices = new ObservableCollection<BTDevice>();
            LoadPairedDevicesCommand = new Command(async () => await ExecuteLoadPairedDevicesCommand());

            PairedDeviceTapped = new Command<BTDevice>(OnPairedDeviceSelected);

            AddPairedDeviceCommand = new Command(OnAddPairedDevice);

            AvailableDevices = new ObservableCollection<AvailableBTDevice>();
            LoadAvailableDevicesCommand = new Command(() => ExecuteLoadAvailableDevicesCommand());
            SearchAvailableDevicesCommand = new Command(() => ExecuteSearchAvailableDevicesCommand());
        }

        async Task ExecuteLoadPairedDevicesCommand()
        {
            IsBusy = true;

            try
            {
                PairedDevices.Clear();
                var devices = await App.DevicesStore.GetAllDevices();
                foreach (var device in devices)
                {
                    PairedDevices.Add(device);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedPairedDevice = null;
        }

        public BTDevice SelectedPairedDevice
        {
            get => _selectedPairedDevice;
            set
            {
                SetProperty(ref _selectedPairedDevice, value);
                OnPairedDeviceSelected(value);
            }
        }

        private async void OnAddPairedDevice(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnPairedDeviceSelected(BTDevice device)
        {
            if (device == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.DeviceID)}={device.Id}");
        }

        private void ExecuteLoadAvailableDevicesCommand()
        {
            AvailableDevices.Clear();
            List<AvailableBTDevice> dev = App.Bluetooth.GetAvailableDevices();
            dev.Sort((x, y) => y.Rssi.CompareTo(x.Rssi));
            dev.ForEach(AvailableDevices.Add);
        }

        private void ExecuteSearchAvailableDevicesCommand()
        {

            int period = 10000;

            TimerCallback timerDelegate = new TimerCallback(async o => {
                await App.Bluetooth.Search(5000);
                ExecuteLoadAvailableDevicesCommand();
                OnPropertyChanged("AvailableDevices");
            });
            Timer timer = new Timer(timerDelegate, null, 0, period);

        }

    }
}