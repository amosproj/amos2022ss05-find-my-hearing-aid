// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using FindMyBLEDevice.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyBLEDevice.Views
{
    public partial class AboutPage : ContentPage
    {
        private AboutViewModel viewModel;

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AboutViewModel();
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