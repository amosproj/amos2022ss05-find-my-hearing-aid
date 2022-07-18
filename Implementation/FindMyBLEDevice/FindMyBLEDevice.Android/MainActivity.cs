// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;
using AndroidX.Core.Content;
using AndroidX.Core.App;

namespace FindMyBLEDevice.Droid
{
    [Activity(Label = "Find My BLE Device", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        readonly int requiredPermissionsRequestCode = 1000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App());


            var requiredPermissions = new[]
            {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation,
                Manifest.Permission.Bluetooth
            };
            // check if the app has permission to access coarse location
            var coarseLocationPermissionGranted =
                ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation);
            // check if the app has permission to access fine location
            var fineLocationPermissionGranted =
                 ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);
            var bluetoothPermissionGranted = 
                ContextCompat.CheckSelfPermission(this, Manifest.Permission.Bluetooth);
            // if either is denied permission, request permission from the user
            if (coarseLocationPermissionGranted == Permission.Denied ||
                fineLocationPermissionGranted == Permission.Denied || 
                bluetoothPermissionGranted == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, requiredPermissions, requiredPermissionsRequestCode);
            }
        }


        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == requiredPermissionsRequestCode)
            {
                // check if you user granted these permissions
                var coarseLocationPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation);
                var fineLocationPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);
                var bluetoothPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.Bluetooth);
                if (coarseLocationPermissionGranted == Permission.Denied ||
                    fineLocationPermissionGranted == Permission.Denied ||
                    bluetoothPermissionGranted == Permission.Denied)
                {
                    var message = "The app is not functioning correctly because the permissions are not granted. You will be redirected to the settings app to grant the required permissions";
                    bool goToSettings = await App.Current.MainPage.DisplayAlert("Attention", message, "Ok", "Cancel");
                    if (goToSettings)
                    {
                        Xamarin.Essentials.AppInfo.ShowSettingsUI();
                    }
                }
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}