// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindMyBLEDevice.ViewModels;
using FindMyBLEDevice.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyBLEDevice.Views
{
    public partial class StrengthPage : ContentPage
    {
        private StrengthViewModel viewModel;
        public StrengthPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new StrengthViewModel();
            //viewModel.PropertyChanged += ViewModelPropertyChanged;
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

        //private async void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs ea)
        //{
        //    if (ea.PropertyName == nameof(viewModel.RadiusDrag))
        //    {
        //        await ellipse.ScaleTo((double)viewModel.Radius / viewModel.RadiusDrag, (uint)viewModel.RssiPollInterval, Easing.Linear);
        //    }
        //}
    }
}