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
            }

            //return ConfigureApp.iOS.InstalledApp("com.amos.findmybledevice").DeviceIdentifier("C6678D33-D7D4-4B7E-9A1B-310249C17CB3").StartApp();
            //return ConfigureApp.iOS.AppBundle("../../FindMyBLEDevice.iOS/bin/iPhoneSimulator/Debug/FindMyBLEDevice.iOS.app").StartApp();
            return ConfigureApp.iOS.AppBundle("../../FindMyBLEDevice.iOS/bin/iPhoneSimulator/Debug/FindMyBLEDevice.iOS.app").DeviceIdentifier("C6678D33-D7D4-4B7E-9A1B-310249C17CB3").StartApp();
        }
    }
}