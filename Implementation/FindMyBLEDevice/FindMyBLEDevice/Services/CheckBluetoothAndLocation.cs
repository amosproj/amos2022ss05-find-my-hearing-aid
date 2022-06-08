// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using FindMyBLEDevice.Services.Location;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice.Services
{
    public static class CheckBluetoothAndLocation
    {
        static public IPlatformSpecificLocation platFormLocationService => DependencyService.Get<IPlatformSpecificLocation>();


        async public static Task Check()
        {
            try
            {
                var locationEnabled = platFormLocationService.IsLocationServiceEnabled();
                var bluetoothEnabled = App.Bluetooth.IsEnabled();

                string message = "For this App to work properly, please enable the following Services:\n";
                message += !locationEnabled ? "\n - Location" : "";
                message += !bluetoothEnabled ? "\n - Bluetooth" : "";
                
                if (!locationEnabled || !bluetoothEnabled)
                {
                    await App.Current.MainPage.DisplayAlert("Information", message, "Ok");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
