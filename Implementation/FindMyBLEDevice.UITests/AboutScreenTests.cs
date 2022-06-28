// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <domi.pysch@gmail.com>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@gmail.com>

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
    public class AboutScreenTests
    {
        IApp app;
        Platform platform;

        public AboutScreenTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void OpenAboutPage()
        {

            // Assert "About"-Screen is shown
            AppResult[] results = app.Query(c => c.Marked("Page_About"));
            Assert.IsTrue(results.Any());

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_About"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_About"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results3 = app.WaitForElement(c => c.Marked("Page_About"));
            Assert.IsTrue(results3.Any());

        }

        [Test]
        public void AboutPageElements()
        {

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_About"));
            Assert.IsTrue(results2.Any());

            // Open devices page
            app.Tap(c => c.Marked("FlyoutItem_About"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results3 = app.WaitForElement(c => c.Marked("Page_About"));
            Assert.IsTrue(results3.Any());

            // Image
            Assert.IsTrue(app.Query(c => c.Marked("AboutPage_Image")).Any());
            // Title
            Assert.IsTrue(app.Query(c => c.Marked("AboutPage_Title")).Any());
            // Description
            Assert.IsTrue(app.Query(c => c.Marked("AboutPage_Description")).Any());
            // Btn Signal
            Assert.IsTrue(app.Query(c => c.Marked("AboutPage_Btn_Signal")).Any());
            // Btn GPS
            Assert.IsTrue(app.Query(c => c.Marked("AboutPage_Btn_GPS")).Any());


        }

        [Test]
        public void ButtonGoToSignal()
        {

            // Assert "About"-Screen is shown
            AppResult[] results = app.Query(c => c.Marked("Page_About"));
            Assert.IsTrue(results.Any());

            // Open devices page
            app.Tap(c => c.Marked("AboutPage_Btn_Signal"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results2 = app.WaitForElement(c => c.Marked("Page_Strength"));
            Assert.IsTrue(results2.Any());

        }

        [Test]
        public void ButtonGoToGPS()
        {

            // Assert "About"-Screen is shown
            AppResult[] results = app.Query(c => c.Marked("Page_About"));
            Assert.IsTrue(results.Any());

            // Open devices page
            app.Tap(c => c.Marked("AboutPage_Btn_GPS"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results2 = app.WaitForElement(c => c.Marked("Page_Map"));
            Assert.IsTrue(results2.Any());

        }


    }
}
