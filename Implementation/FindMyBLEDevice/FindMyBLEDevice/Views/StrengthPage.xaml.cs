// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindMyBLEDevice.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyBLEDevice.Views
{
    public partial class StrengthPage : ContentPage
    {
        public StrengthPage()
        {
            InitializeComponent();
            BindingContext = new StrengthViewModel();

        }
    }
}