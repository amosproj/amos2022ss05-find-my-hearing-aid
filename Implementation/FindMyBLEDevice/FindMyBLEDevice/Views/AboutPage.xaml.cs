// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.ViewModels;
using Xamarin.Forms;

namespace FindMyBLEDevice.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = new AboutViewModel(App.Navigator);
        }
    }
}
