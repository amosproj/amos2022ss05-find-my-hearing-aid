// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindMyBLEDevice.Services;
using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel()
        {
            Title = "MapSearch";
        }

        public async Task OnAppearing()
        {
            await CheckBluetoothAndLocation.Check();
        }

    }
}