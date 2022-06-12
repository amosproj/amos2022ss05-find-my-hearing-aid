using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                // return ConfigureApp.Android.ApkFile("com.amos.findmybledevice").StartApp();

                return ConfigureApp.Android.InstalledApp("com.amos.findmybledevice").StartApp();
                // return ConfigureApp.Android.StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}