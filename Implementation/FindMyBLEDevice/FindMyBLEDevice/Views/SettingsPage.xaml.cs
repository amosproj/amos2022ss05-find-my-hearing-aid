// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@fau.de>

using FindMyBLEDevice.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyBLEDevice.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel(App.Settings, App.DevicesStore);
        }
    }
}
