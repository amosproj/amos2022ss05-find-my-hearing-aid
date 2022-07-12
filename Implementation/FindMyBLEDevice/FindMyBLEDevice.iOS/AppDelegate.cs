using System;
using System.Collections.Generic;
using System.Linq;
using CoreLocation;
using Foundation;
using UIKit;

namespace FindMyBLEDevice.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            #if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
            #endif

            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            LoadApplication(new App());

            switch (CLLocationManager.Status)
            {
                case CLAuthorizationStatus.Authorized | CLAuthorizationStatus.AuthorizedAlways | CLAuthorizationStatus.AuthorizedWhenInUse:
                    Console.WriteLine("Access");
                    break;
                default:
                    Console.WriteLine("No Access");
                    var message = "The app is not functioning correctly because the permissions are not granted. You will be redirected to the settings app to grant the required permissions";
                    bool goToSettings = App.Current.MainPage.DisplayAlert("Attention", message, "Ok", "Cancel").GetAwaiter().GetResult();
                    if (goToSettings)
                    {
                        Xamarin.Essentials.AppInfo.ShowSettingsUI();
                    }
                    break;
            }

            return base.FinishedLaunching(app, options);
        }
    }
}
