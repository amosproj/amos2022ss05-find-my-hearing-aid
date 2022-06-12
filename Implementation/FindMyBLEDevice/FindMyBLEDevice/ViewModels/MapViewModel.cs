// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using FindMyBLEDevice.Models;

namespace FindMyBLEDevice.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel()
        {
            Title = "MapSearch";
        }
        public BTDevice Device
        {
            get => App.DevicesStore.SelectedDevice;
        }

        public void OnAppearing() { }
        public void OnDisappearing() { }
    }
}