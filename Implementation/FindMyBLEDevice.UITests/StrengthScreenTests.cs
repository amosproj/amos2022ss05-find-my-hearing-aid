// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <domi.pysch@gmail.com>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@gmail.com>

using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace FindMyBLEDevice.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class StrengthScreenTests
    {
        IApp app;
        Platform platform;

        public StrengthScreenTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void OpenStrengthPage()
        {

            // Assert "About"-Screen is shown
            AppResult[] results = app.Query(c => c.Marked("Page_About"));
            Assert.IsTrue(results.Any());

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Strength"));
            Assert.IsTrue(results2.Any());

            // Open strength page
            app.Tap(c => c.Marked("FlyoutItem_Strength"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results3 = app.WaitForElement(c => c.Marked("Page_Strength"));
            Assert.IsTrue(results3.Any());

        }


        [Test]
        public void StrengthPageElements()
        {

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Strength"));
            Assert.IsTrue(results2.Any());

            // Open strength page
            app.Tap(c => c.Marked("FlyoutItem_Strength"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results3 = app.WaitForElement(c => c.Marked("Page_Strength"));
            Assert.IsTrue(results3.Any());

            // Label
            Assert.IsTrue(app.Query(c => c.Marked("StrengthPage_LabelStatus")).Any());

            // Circle
            AppResult[] result = app.Query(c => c.Marked("StrengthPage_FilledCircle"));
            Assert.IsTrue(result.Any());

            // Selected Device Frame
            // on some devices not visible right away
            app.ScrollDown();

            Assert.IsTrue(app.Query(c => c.Marked("StrengthPage_Frame")).Any());
            Assert.IsTrue(app.Query(c => c.Marked("StrengthPage_SelectedDeviceLabel")).Any());

        }

        [Test]
        public void StrengthPageNoDeviceSelectedOnStart()
        {

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Strength"));
            Assert.IsTrue(results2.Any());

            // Open strength page
            app.Tap(c => c.Marked("FlyoutItem_Strength"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results = app.WaitForElement(c => c.Marked("Page_Strength"));
            Assert.IsTrue(results.Any());
            
            // on some devices not visible right away
            app.ScrollDown();
            Assert.AreEqual("Please select a device first", app.Query(c => c.Marked("StrengthPage_SelectedDeviceLabel"))[0].Text);

        }

        [Test]
        public void StrengthPageCircles()
        {

            // Open navigation drawer
            app.SwipeLeftToRight(0.99);

            // Wait for drawer
            AppResult[] results2 = app.WaitForElement(c => c.Marked("FlyoutItem_Strength"));
            Assert.IsTrue(results2.Any());

            // Open strength page
            app.Tap(c => c.Marked("FlyoutItem_Strength"));

            // Assert that devices page (or at least one element from the page) is visible
            AppResult[] results = app.WaitForElement(c => c.Marked("Page_Strength"));
            Assert.IsTrue(results.Any());

            // Circles
            AppResult[] circles = app.Query(c => c.Marked("StrengthPage_CircleLayout").Child());

            // Check all circles are in 1:1 ratio
            for (int i = 0; i < circles.Length; i++)
            {
                Assert.AreEqual(circles[i].Rect.Width, circles[i].Rect.Height);
            }

            var content = app.Query("content").First();
            float ScreenWidth = content.Rect.Width;
            // Check all circles are centered
            for (int i = 0; i < circles.Length; i++)
            {
                Assert.IsTrue(Math.Abs(ScreenWidth / 2 - circles[i].Rect.CenterX) < 2f);
            }


        }


    }
}
