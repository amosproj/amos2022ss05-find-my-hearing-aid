// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@gmx.net>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>

using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using FindMyBLEDevice.Services.Location;


namespace FindMyBLEDevice.Services
{
    public class CheckBluetoothAndLocationService
    {
        private readonly int checkInterval = 5000;
        private bool running;
        static public IPlatformSpecificLocation platFormLocationService => DependencyService.Get<IPlatformSpecificLocation>();

        public CheckBluetoothAndLocationService()
        {
            running = false;
        }

        public void Start()
        {
            if (running) return;

            running = true;
            Task.Run(RunAsync);
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Started Checking Services");



            while (running)
            {
                try //just to be safe...
                {

                    try
                    {

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            bool hasBluetoothPermission = true;
                            bool hasLocationPermission = false;  // checkLocationPermission();

                            if (!hasBluetoothPermission || !hasLocationPermission)
                            {
                                string msg = "The app is not functioning correctly because the permissions are not granted. Please navigate to the settings app to grant the required permissions";
                                bool goToSettings = await Xamarin.Forms.Device.InvokeOnMainThreadAsync(async () => {
                                    return await Application.Current.MainPage.DisplayAlert("Attention", msg, "Ok", "Cancel");
                                });
                            }
                        }

                        var locationEnabled = platFormLocationService.IsLocationServiceEnabled();
                        var bluetoothEnabled = App.Bluetooth.IsEnabled();

                        string message = "If Bluetooth and GPS are not activated the correct functionality of this app cannot be guaranteed.\n";
                        message += "Please enable the following service";
                        message += !locationEnabled && !bluetoothEnabled ? "s:\n" : ":\n";
                        message += !bluetoothEnabled ? "\n - Bluetooth" : "";
                        message += !locationEnabled ? "\n - GPS" : "";

                        if (!locationEnabled || !bluetoothEnabled)
                        {
                            await Xamarin.Forms.Device.InvokeOnMainThreadAsync(async () => {
                                await Application.Current.MainPage.DisplayAlert("Attention", message, "Ok");
                            });
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    await Task.Delay(checkInterval);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
