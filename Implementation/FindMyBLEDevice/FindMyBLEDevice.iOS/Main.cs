// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System;
using System.Collections.Generic;
using System.Linq;
using CoreLocation;
using Foundation;
using UIKit;

namespace FindMyBLEDevice.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static async void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, typeof(AppDelegate));

            switch (CLLocationManager.Status)
            {
                case CLAuthorizationStatus.Authorized | CLAuthorizationStatus.AuthorizedAlways | CLAuthorizationStatus.AuthorizedWhenInUse:
                    Console.WriteLine("Access");
                    break;
                default:
                    Console.WriteLine("No Access");
                    var message = "The app is not functioning correctly because the permissions are not granted. You will be redirected to the settings app to grant the required permissions";
                    bool goToSettings = await App.Current.MainPage.DisplayAlert("Attention", message, "Ok", "Cancel");
                    if (goToSettings)
                    {
                        Xamarin.Essentials.AppInfo.ShowSettingsUI();
                    }
                    break;
            }
        }
    }
}
