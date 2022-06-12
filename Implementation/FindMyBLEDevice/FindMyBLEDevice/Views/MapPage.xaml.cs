// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using Xamarin.Forms;
using FindMyBLEDevice.ViewModels;


namespace FindMyBLEDevice.Views
{
    public partial class MapPage : ContentPage
    {
        private MapViewModel viewModel;
        public MapPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MapViewModel();
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