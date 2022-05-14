// SPDX-License-Identifier: MIT
// SDPX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SDPX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@googlemail.com>

using FindMyBLEDevice.Services;
using FindMyBLEDevice.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyBLEDevice
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
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
