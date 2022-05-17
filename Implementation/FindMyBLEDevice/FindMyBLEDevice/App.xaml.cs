// SPDX-License-Identifier: MIT
// SDPX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SDPX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@googlemail.com>

using FindMyBLEDevice.Services;
using FindMyBLEDevice.Services.Database;
using System;
using Xamarin.Forms;

namespace FindMyBLEDevice
{
    public partial class App : Application
    {

        // Interface to stored BTDevices
        private static IDevicesStore devicesStore;

        // Create the devices store as a singleton.
        public static IDevicesStore DevicesStore
        {
            get
            {
                if (devicesStore == null)
                {
                    devicesStore = new DevicesStore();
                }
                return devicesStore;
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            throw new NotSupportedException();
        }

        protected override void OnSleep()
        {
            throw new NotSupportedException();
        }

        protected override void OnResume()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// This method was created for setting up unit tests. 
        /// It may be deleted in the future.
        /// </summary>
        /// <param name="a">A number</param>
        /// <param name="b">Another number</param>
        /// <returns>The sum of those numbers</returns>
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
