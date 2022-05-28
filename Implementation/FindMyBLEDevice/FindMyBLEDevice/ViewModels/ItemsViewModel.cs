using FindMyBLEDevice.Models;
using FindMyBLEDevice.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;


namespace FindMyBLEDevice.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private BTDevice _selectedItem;

        public ObservableCollection<BTDevice> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<BTDevice> ItemTapped { get; }
        public Command<AvailableBTDevice> FoundDeviceTapped { get; }

        private ObservableCollection<AvailableBTDevice> _AvailableDevices;
        public ObservableCollection<AvailableBTDevice> AvailableDevices {
            get { 
                return _AvailableDevices; 
            }
            set
            {
                List<AvailableBTDevice> devices = value.ToList();
                devices.Sort((x, y) => y.Rssi.CompareTo(x.Rssi));
                _AvailableDevices = new ObservableCollection<AvailableBTDevice>(devices);                
                OnPropertyChanged("AvailableDevices");
            }
        }

        public Command LoadDevicesCommand { get; }
        public Command SearchDevicesCommand { get; }

        public ItemsViewModel()
        {
            Title = "Devices";
            Items = new ObservableCollection<BTDevice>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<BTDevice>(OnItemSelected);
            FoundDeviceTapped = new Command<AvailableBTDevice>(SelectFoundDevice);

            AddItemCommand = new Command(OnAddItem);

            AvailableDevices = new ObservableCollection<AvailableBTDevice>();
            SearchDevicesCommand = new Command(() => ExecuteSearchDevicesCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await App.DevicesStore.GetAllDevices();
                foreach (var item in items)
                {
                    Items.Add(item);
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
            SelectedItem = null;
        }

        public BTDevice SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(BTDevice device)
        {
            if (device == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.DeviceID)}={device.Id}");
        }

        async void SelectFoundDevice(AvailableBTDevice device)
        {
            if (device == null)
                return;

            await App.Bluetooth.StopSearch();

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(ItemDetailViewModel.DeviceID)}={device.Id}");
        }


        private async Task ExecuteSearchDevicesCommand()
        {
            await App.Bluetooth.Search(20000, AvailableDevices);
        }

    }
}