// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>

using FindMyBLEDevice.ViewModels;
using FindMyBLEDevice.XamarinAccess;
using Xamarin.Forms;

namespace FindMyBLEDevice.Views
{
    public partial class StrengthPage : ContentPage
    {
        private StrengthViewModel viewModel;

        public StrengthPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new StrengthViewModel(new DeviceDisplayAccess(), App.DevicesStore, App.Bluetooth, App.Navigator);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.OnDisappearing();
        }
    }
}