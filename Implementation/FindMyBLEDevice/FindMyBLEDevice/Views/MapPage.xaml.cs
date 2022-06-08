// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FindMyBLEDevice.ViewModels;



namespace FindMyBLEDevice.Views
{
    public partial class MapPage : ContentPage
    {
        private MapViewModel _viewModel;
        public MapPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MapViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }
    }
}