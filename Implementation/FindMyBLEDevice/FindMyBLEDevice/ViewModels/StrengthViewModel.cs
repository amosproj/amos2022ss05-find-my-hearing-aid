// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace FindMyBLEDevice.ViewModels
{
    [QueryProperty(nameof(BT_id), "bt_id")]
    public class StrengthViewModel : BaseViewModel
    {
        private string bt_id;
        
        public StrengthViewModel()
        {
            Title = "StrengthSearch";
        }

        public string BT_id { 
            get => bt_id; 
            set => SetProperty(ref bt_id, value); 
        }
    }
}