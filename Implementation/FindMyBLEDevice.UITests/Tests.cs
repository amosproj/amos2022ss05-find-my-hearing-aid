using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace FindMyBLEDevice.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }


        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        /*
        
            Important Note: Mind that you always have to have installed the latest version of the app!
                    Running tests does NOT push the app to the running emulator, it justs starts the existing version.

            Some examples:

            Interactive mode: useful for setting up tests.
            `app.Repl();`

            Search ("query") (until it shows up, in case of animations etc.) for UI-Elements via their "AutomationId". 
            `AppResult[] results = app.WaitForElement(c => c.Marked("Btn_LearnMore"));`

            Search ("query") for UI-Elements via their "AutomationId". 
            `appResult[] results2 = app.Query(c => c.Marked("Btn_LearnMore"));`
            `Assert.IsTrue(results.Any());`

            Mimick user actions
            `app.Tap(c => c.Marked("Btn_LearnMore"));`

         */


        [Test]
        public void OpenDevicesPage()
        {

            // Assert "About"-Screen is shown
            AppResult[] results = app.Query(c => c.Marked("Btn_LearnMore"));
            Assert.IsTrue(results.Any());

            // Tap on menu drawer (named "ok" for some reason..)
            app.Tap(c => c.Marked("OK"));

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Devices"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_Devices"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results3 = app.WaitForElement(c => c.Marked("Label_SavedDevices"));
            Assert.IsTrue(results3.Any());

        }


    }
}
