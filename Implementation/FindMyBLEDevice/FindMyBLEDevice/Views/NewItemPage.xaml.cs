// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>

using FindMyBLEDevice.ViewModels;
using Xamarin.Forms;

namespace FindMyBLEDevice.Views
{
    public partial class NewItemPage : ContentPage
    {
        NewItemViewModel _viewModel;

        public NewItemPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new NewItemViewModel(App.Navigator, App.DevicesStore, App.Geolocation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
