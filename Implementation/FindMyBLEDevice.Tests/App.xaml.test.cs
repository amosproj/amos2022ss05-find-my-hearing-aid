// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@fau.de>

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
    }
}