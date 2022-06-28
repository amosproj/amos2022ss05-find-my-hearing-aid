// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <domi.pysch@gmail.com>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@gmail.com>

using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace FindMyBLEDevice.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class DevicesScreenTests
    {
        IApp app;
        Platform platform;

        public DevicesScreenTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void OpenDevicesPage()
        {

            // Assert "About"-Screen is shown
            AppResult[] results = app.Query(c => c.Marked("Page_About"));
            Assert.IsTrue(results.Any());

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Devices"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_Devices"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results3 = app.WaitForElement(c => c.Marked("Page_Devices"));
            Assert.IsTrue(results3.Any());

        }


        [Test]
        public void DevicesPaggieElements()
        {

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Devices"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_Devices"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results3 = app.WaitForElement(c => c.Marked("Page_Devices"));
            Assert.IsTrue(results3.Any());

            // Search for elements
            Assert.IsTrue(app.Query(c => c.Marked("DevicesPage_LabelSavedDevices")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("DevicesPage_LabelAvailableDevices")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("DevicesPage_ButtonScan")).Any());

        }

    }
}
