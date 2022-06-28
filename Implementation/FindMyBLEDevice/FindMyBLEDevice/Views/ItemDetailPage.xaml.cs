// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using FindMyBLEDevice.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace FindMyBLEDevice.Views
{
    public partial class ItemDetailPage : ContentPage
    {

        ItemDetailViewModel _viewModel;

        private bool initialized = false;

        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemDetailViewModel();
            initialized = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }


        private void UserLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (initialized)
            {
                _viewModel.UserLabel_TextChanged();
            }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.OnDisappearing();
        }
    }
}