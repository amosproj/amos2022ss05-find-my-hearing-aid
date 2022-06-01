// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using FindMyBLEDevice.Models;
using System.Diagnostics;

namespace FindMyBLEDevice.ViewModels
{
    [QueryProperty(nameof(DeviceID), nameof(DeviceID))]
    public class StrengthViewModel : BaseViewModel
    {
        private int _radius;
        private int _meter;
        private string _name = "Someone's AirPods Pro";
        private int _currentRssi;
        private string _bt_id;
        private int _deviceID;
        public int Id { get; set; }
        public StrengthViewModel()
        {
            Title = "StrengthSearch";
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string BT_id
        {
            get => _bt_id;
            set => SetProperty(ref _bt_id, value);
        }

        public int Radius
        {
            get => _radius;
            set => SetProperty(ref _radius, value);
        }
        public int Meter
        {
            get => _meter;
            set => SetProperty(ref _meter, value);
        }

        public int CurrentRssi
        {
            get => _currentRssi;
            set => SetProperty(ref _currentRssi, value);
        }
        public int DeviceID
        {
            get
            {
                return _deviceID;
            }
            set
            {
                _deviceID = value;
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