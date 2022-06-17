﻿// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@googlemail.com>
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Services;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Location;
using System;
using Xamarin.Forms;

namespace FindMyBLEDevice
{
    public partial class App : Application
    {

        // Interface to stored BTDevices
        private static IDevicesStore devicesStore;
        // Encapsulation of bluetooth functionality
        private static IBluetooth bluetooth;
        // Interface to stored location Permission
        private static ILocation location;

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
        public static IBluetooth Bluetooth
        {
            get
            {
                if (bluetooth == null)
                {
                    bluetooth = new Bluetooth();
                }
                return bluetooth;
            }
        }

        public static ILocation Location
        {
            get
            {
                if (location == null)
                {
                    location = new Location();
                }
                return location;
            }
        }



        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        // TODO: Remove the following functions

        /*
        protected override void OnStart()
        {
            // comment to make the linter happy
        }

        protected override void OnSleep()
        {
            // comment to make the linter happy
        }

        protected override void OnResume()
        {
            // comment to make the linter happy
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
        } */
    }
}