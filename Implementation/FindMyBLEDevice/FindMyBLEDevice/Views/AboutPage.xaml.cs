// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>

using FindMyBLEDevice.ViewModels;
using Xamarin.Forms;

namespace FindMyBLEDevice.Views
{
    public partial class AboutPage : ContentPage
    {
        private readonly AboutViewModel viewModel;

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AboutViewModel(App.Navigator, App.DevicesStore);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}
