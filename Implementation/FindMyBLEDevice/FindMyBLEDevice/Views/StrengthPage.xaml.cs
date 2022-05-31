// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using System;
using System.Collections.Generic;
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
        public StrengthPage()
        {
            InitializeComponent();
            BindingContext = new StrengthViewModel();
        }
        
        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;
            ellipse.WidthRequest = value;
            ellipse.HeightRequest = value;
            int output = ((int)value - 30) * 6 / 370 + 1;
            sliderLabel.Text = String.Format("Hot/Cold within ~{0}m", output);
        }

    }
}