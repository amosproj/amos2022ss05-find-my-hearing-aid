// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using FindMyBLEDevice.Services.Settings;
using FindMyBLEDevice.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class SettingsViewModelTests
    {
        [TestMethod]
        public void Constructor_Works()
        {
            // arrange
            var st = new Mock<ISettings>();
            st.Setup(mock => mock.Get(It.IsAny<string>(), It.IsAny<int>())).Returns(0);
            SettingsViewModel vm;


            // act
            vm = new SettingsViewModel(st.Object, null, Mock.Of<IDevicesStore>());

            // assert
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.RssiIntervalString);
            Assert.IsNotNull(vm.UpdateServiceIntervalString);
        }

        [TestMethod]
        public void DisplayNamelessDevices_GetsSettings()
        {
            // arrange
            var st1 = new Mock<ISettings>();
            st1.Setup(mock => mock.Get(It.Is<string>(s => s == SettingsNames.DisplayNamelessDevices), It.IsAny<bool>())).Returns(false);
            SettingsViewModel vm1 = new SettingsViewModel(st1.Object, null, Mock.Of<IDevicesStore>());
            var st2 = new Mock<ISettings>();
            st2.Setup(mock => mock.Get(It.Is<string>(s => s == SettingsNames.DisplayNamelessDevices), It.IsAny<bool>())).Returns(true);
            SettingsViewModel vm2 = new SettingsViewModel(st2.Object, null, Mock.Of<IDevicesStore>());

            // act
            var res1 = vm1.DisplayNamelessDevices;
            var res2 = vm2.DisplayNamelessDevices;

            // assert
            st1.Verify(mock => mock.Get(It.Is<string>(s => s == SettingsNames.DisplayNamelessDevices), It.IsAny<bool>()), Times.Once);
            Assert.AreEqual(false, res1);
            st2.Verify(mock => mock.Get(It.Is<string>(s => s == SettingsNames.DisplayNamelessDevices), It.IsAny<bool>()), Times.Once);
            Assert.AreEqual(true, res2);
        }

        [TestMethod]
        public void DisplayNamelessDevices_SetsSettings()
        {
            // arrange
            var res1 = false;
            var st1 = new Mock<ISettings>();
            st1.Setup(mock => mock.Set(It.Is<string>(s => s == SettingsNames.DisplayNamelessDevices), It.IsAny<bool>()))
                .Callback<string, bool>((name, value) => res1 = value);
            SettingsViewModel vm1 = new SettingsViewModel(st1.Object, null, Mock.Of<IDevicesStore>());
            var res2 = true;
            var st2 = new Mock<ISettings>();
            st2.Setup(mock => mock.Set(It.Is<string>(s => s == SettingsNames.DisplayNamelessDevices), It.IsAny<bool>()))
                .Callback<string, bool>((name, value) => res2 = value);
            SettingsViewModel vm2 = new SettingsViewModel(st2.Object, null, Mock.Of<IDevicesStore>());

            // act
            vm1.DisplayNamelessDevices = true;
            vm2.DisplayNamelessDevices = false;

            // assert
            st1.Verify(mock => mock.Set(It.Is<string>(s => s == SettingsNames.DisplayNamelessDevices), It.IsAny<bool>()), Times.Once);
            Assert.AreEqual(true, res1);
            st2.Verify(mock => mock.Set(It.Is<string>(s => s == SettingsNames.DisplayNamelessDevices), It.IsAny<bool>()), Times.Once);
            Assert.AreEqual(false, res2);
        }

        //TODO: check if all the other properties actually get and set the settings 
        //TODO: check whether properties not present in settings return the respective default value
        //TODO: check whether setting the string-variants of intervals changes the int values and vice versa
        //TODO: check whether interval-values stay within their respective min and max 
    }
}