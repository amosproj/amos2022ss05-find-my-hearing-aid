// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
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
            Title = "Available BLE Devices";
            AvailableDevices = new ObservableCollection<AvailableBTDevice>();
            LoadDevicesCommand = new Command(() => ExecuteLoadDevicesCommand());
            SearchDevicesCommand = new Command(() => ExecuteSearchDevicesCommand());
        }

        private void ExecuteLoadDevicesCommand()
        {
            AvailableDevices.Clear();
            List<AvailableBTDevice> dev = App.Bluetooth.GetAvailableDevices();
            dev.Sort((x, y) => y.Rssi.CompareTo(x.Rssi));
            dev.ForEach(AvailableDevices.Add);
        }

        private void ExecuteSearchDevicesCommand()
        {

            int period = 10000;

            TimerCallback timerDelegate = new TimerCallback(async o => {
                await App.Bluetooth.Search(5000);
                ExecuteLoadDevicesCommand();
                OnPropertyChanged("AvailableDevices");
            });
            Timer timer = new Timer(timerDelegate, null, 0, period);

        }

    }
}
