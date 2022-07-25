// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using Xamarin.Forms;
using FindMyBLEDevice.ViewModels;
using FindMyBLEDevice.XamarinAccess;

namespace FindMyBLEDevice.Views
{
    public partial class MapPage : ContentPage
    {
        private MapViewModel viewModel;

        public MapPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MapViewModel(map, App.Geolocation, App.Navigator, App.DevicesStore, new DeviceAccess());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.OnDisappearing();
        }
    }
}
