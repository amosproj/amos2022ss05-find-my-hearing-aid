// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
using FindMyBLEDevice.Views;
using System;
using Xamarin.Forms;

namespace FindMyBLEDevice
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(StrengthPage), typeof(StrengthPage));
            Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            //prevents the back-button from closing the app
            if (string.IsNullOrEmpty(args.Target.Location.ToString()))
            {
                args.Cancel();
                return;
            }
            base.OnNavigating(args);
        }

    }
}
