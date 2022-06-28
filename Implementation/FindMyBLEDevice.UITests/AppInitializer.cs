// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <domi.pysch@gmail.com>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@gmail.com>

using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace FindMyBLEDevice.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.InstalledApp("com.amos.findmybledevice").StartApp();
                // return ConfigureApp.Android.StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}