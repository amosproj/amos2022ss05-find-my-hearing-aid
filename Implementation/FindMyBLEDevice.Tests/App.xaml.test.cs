// SPDX-License-Identifier: MIT
// SDPX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SDPX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@googlemail.com>

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FindMyBLEDevice.Tests
{
    [TestClass]
    public class AppTest
    {
        /// <summary>
        /// Setup necessary to create an instance of FindMyBLEDevice.App, because that class has Xamarin.Forms dependencies. 
        /// See http://jonathanpeppers.com/Blog/mocking-xamarin-forms for details.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
        }

        /// <summary>
        /// This test was created for setting up unit tests. 
        /// It may be deleted in the future.
        /// </summary>
        [TestMethod]
        public void TestAdd()
        {
            int a = 1;
            int b = 2;
            int expected = 3;
            App app = new();

            int result = app.Add(a, b);
            Assert.AreEqual(expected, result);
        }
    }
}