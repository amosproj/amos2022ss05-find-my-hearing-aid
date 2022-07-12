// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@googlemail.com>
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Location;
using FindMyBLEDevice.Services.Geolocation;
using FindMyBLEDevice.Services.Settings;
using Xamarin.Forms;
using FindMyBLEDevice.Views;
using FindMyBLEDevice.Services;

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
        // Interface to access Geolocation
        private static IGeolocation geolocation;
        // Interface to stored settings 
        private static ISettings settings;
        // Update-service
        private static UpdateService updateService;
        // Interface to access Shell navigation
        private static INavigator navigator;
        // Check-service
        private static CheckBluetoothAndLocationService checkService;

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
                    bluetooth = new Bluetooth(Settings);
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

        public static IGeolocation Geolocation
        {
            get
            {
                if (geolocation == null)
                {
                    geolocation = new Geolocation();
                }
                return geolocation;
            }
        }

        public static ISettings Settings
        {
            get
            {
                if(settings == null)
                {
                    settings = new Settings();
                }
                return settings;
            }
        }

        public static UpdateService UpdateService
        {
            get
            {
                if (updateService == null)
                {
                    updateService = new UpdateService(Bluetooth, DevicesStore, Geolocation, Settings);
                }
                return updateService;
            }
        }

        public static INavigator Navigator
        {
            get
            {
                if(navigator == null)
                {
                    navigator = new Navigator(
                        nameof(AboutPage), 
                        nameof(ItemsPage),
                        nameof(NewItemPage),
                        nameof(ItemDetailPage),
                        nameof(StrengthPage),
                        nameof(MapPage),
                        nameof(SettingsPage));
                }
                return navigator;
            }
        }

        
        public static CheckBluetoothAndLocationService CheckService
        {
            get
            {
                if(checkService == null)
                {
                    checkService = new CheckBluetoothAndLocationService();
                }
                return checkService;
            }
        }


        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            CheckService.Start();
            UpdateService.Start();
        }
    }
}
