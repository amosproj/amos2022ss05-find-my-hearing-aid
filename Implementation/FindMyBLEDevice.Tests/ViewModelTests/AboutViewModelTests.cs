// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class AboutViewModelTests
    {
        [TestMethod]
        public void Constructor_Works()
        {
            // arrange
            AboutViewModel vm;

            // act
            vm = new AboutViewModel(new Mock<INavigator>().Object);

            // assert
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.OpenMapPageCommand);
            Assert.IsNotNull(vm.OpenStrengthPageCommand);
        }
    }
}