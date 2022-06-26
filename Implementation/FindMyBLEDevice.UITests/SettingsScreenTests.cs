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
    public class SettingsScreenTests
    {
        IApp app;
        Platform platform;

        public SettingsScreenTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void OpenSettingsPage()
        {

            // Assert "About"-Screen is shown
            AppResult[] results = app.Query(c => c.Marked("Page_About"));
            Assert.IsTrue(results.Any());

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Settings"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_Settings"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results3 = app.WaitForElement(c => c.Marked("Page_Settings"));
            Assert.IsTrue(results3.Any());

        }


        [Test]
        public void SettingsPageElements()
        {

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Settings"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_Settings"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results3 = app.WaitForElement(c => c.Marked("Page_Settings"));
            Assert.IsTrue(results3.Any());

            // Setting: Show devices without adv. name
            Assert.IsTrue(app.Query(c => c.Marked("Setting_NoAdvName_Label")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("Setting_NoAdvName_Switch")).Any());
            // Setting: Show devices with weak signal
            Assert.IsTrue(app.Query(c => c.Marked("Setting_WeakConn_Label")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("Setting_WeakConn_Switch")).Any());
            // Setting: rssi-update-interval
            Assert.IsTrue(app.Query(c => c.Marked("Setting_RSSIInterval_Label")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("Setting_RSSIInterval_Entry")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("Setting_RSSIInterval_Slider")).Any());

        }


        [Test]
        public void Settings_ShowDevWithoutAdvName_Switch()
        {

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Settings"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_Settings"));
            app.WaitForElement(c => c.Marked("Page_Settings"));

            // Setting: Show devices without adv. name
            Assert.IsTrue(app.Query(c => c.Marked("Setting_NoAdvName_Label")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("Setting_NoAdvName_Switch")).Any());

            // Tap switch: should change value.
            string switchValue = "";
            if (platform == Platform.Android)
            {
                switchValue = "isChecked";
            } else if (platform == Platform.iOS)
            {
                switchValue = "isOn";
            }

            bool b = app.Query(c => c.Marked("Setting_NoAdvName_Switch").Invoke(switchValue).Value<bool>()).First();
            app.Tap(c => c.Marked("Setting_NoAdvName_Switch"));
            bool b2 = app.Query(c => c.Marked("Setting_NoAdvName_Switch").Invoke(switchValue).Value<bool>()).First();
            Assert.IsFalse(b == b2);

        }

        [Test]
        public void Settings_ShowDevWeakConn_Switch()
        {

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Settings"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_Settings"));
            app.WaitForElement(c => c.Marked("Page_Settings"));

            // Setting: Show devices without adv. name
            Assert.IsTrue(app.Query(c => c.Marked("Setting_WeakConn_Label")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("Setting_WeakConn_Switch")).Any());

            // Tap switch: should change value.
            string switchValue = "";
            if (platform == Platform.Android)
            {
                switchValue = "isChecked";
            }
            else if (platform == Platform.iOS)
            {
                switchValue = "isOn";
            }

            bool b = app.Query(c => c.Marked("Setting_WeakConn_Switch").Invoke(switchValue).Value<bool>()).First();
            app.Tap(c => c.Marked("Setting_WeakConn_Switch"));
            bool b2 = app.Query(c => c.Marked("Setting_WeakConn_Switch").Invoke(switchValue).Value<bool>()).First();
            Assert.IsFalse(b == b2);

        }

        [Test]
        public void Settings_RSSIInterval_Slider()
        {

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Settings"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_Settings"));
            app.WaitForElement(c => c.Marked("Page_Settings"));

            Assert.IsTrue(app.Query(c => c.Marked("Setting_RSSIInterval_Slider")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("Setting_RSSIInterval_Entry")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("Setting_RSSIInterval_Label")).Any());

            app.ClearText("Setting_RSSIInterval_Entry");
            app.EnterText("Setting_RSSIInterval_Entry", "100");
            string s2 = app.Query(c => c.Marked("Setting_RSSIInterval_Entry"))[0].Text;
            Assert.IsTrue(s2 == "100");

            /*
             * This does not work for some reason. 
             * app.SetSliderValue("Setting_RSSIInterval_Slider", 600);
             * string s3 = app.Query(c => c.Marked("Setting_RSSIInterval_Entry"))[0].Text;
             * Assert.IsTrue(s3 == "600");
            */

        }


    }
}
