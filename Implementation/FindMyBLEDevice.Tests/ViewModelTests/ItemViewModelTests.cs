// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Location;
using FindMyBLEDevice.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class ItemViewModelTests
    {
        [TestMethod]
        public void SavedDevicesList_ReactsToChangesInDatabase()
        {
            // arrange
            var dev1 = new BTDevice();
            var dev2 = new BTDevice();
            var ds = new Mock<IDevicesStore>();
            ds.SetupSequence(mock => mock.GetAllDevices())
                .Returns(Task.FromResult(new List<BTDevice>(new BTDevice[] { dev1 })))
                .Returns(Task.FromResult(new List<BTDevice>(new BTDevice[] { dev1, dev2 })))
                .Returns(Task.FromResult(new List<BTDevice>(new BTDevice[] { dev1 })));

            var vm = new ItemsViewModel(Mock.Of<INavigator>(), ds.Object, Mock.Of<IBluetooth>(), Mock.Of<ILocation>());

            // act
            vm.OnAppearing();
            var firstState = vm.SavedDevices;
            ds.Raise(mock => mock.DevicesChanged += null, null, It.IsAny<List<int>>());
            var secondState = vm.SavedDevices;
            ds.Raise(mock => mock.DevicesChanged += null, null, It.IsAny<List<int>>());
            var thirdState = vm.SavedDevices;
            vm.OnDisappearing();

            // assert
            ds.Verify(mock => mock.GetAllDevices(), Times.Exactly(3));
            ds.VerifyAdd(mock => mock.DevicesChanged += It.IsAny<EventHandler<List<int>>>(), Times.Once);
            ds.VerifyRemove(mock => mock.DevicesChanged -= It.IsAny<EventHandler<List<int>>>(), Times.Once);
            Assert.AreEqual(1, firstState.Count);
            Assert.AreEqual(2, secondState.Count);
            Assert.AreEqual(1, thirdState.Count);
        }

        [TestMethod]
        public void AvailableDevicesList_ReactsToDiscoveredDevices()
        {
            // arrange
            var dev1 = new BTDevice {BT_GUID = "1"};
            var dev2 = new BTDevice {BT_GUID = "2"};
            var bt = new Mock<IBluetooth>();

            var vm = new ItemsViewModel(Mock.Of<INavigator>(), Mock.Of<IDevicesStore>(), bt.Object, Mock.Of<ILocation>());

            // act
            vm.OnAppearing();
            int firstCount = vm.AvailableDevices.Count;
            bt.Raise(mock => mock.DeviceDiscovered += null, null, dev1);
            int secondCount = vm.AvailableDevices.Count;
            bt.Raise(mock => mock.DeviceDiscovered += null, null, dev2);
            int thirdCount = vm.AvailableDevices.Count;
            vm.OnDisappearing();

            // assert
            bt.VerifyAdd(mock => mock.DeviceDiscovered += It.IsAny<EventHandler<BTDevice>>(), Times.Once);
            bt.VerifyRemove(mock => mock.DeviceDiscovered -= It.IsAny<EventHandler<BTDevice>>(), Times.Once);
            Assert.AreEqual(0, firstCount);
            Assert.AreEqual(1, secondCount);
            Assert.AreEqual(2, thirdCount);
        }

        [TestMethod]
        public void AvailableDevicesList_ThrowsAwayGuidDuplicates()
        {
            // arrange
            var dev1 = new BTDevice {BT_GUID = "1"};
            var dev2 = new BTDevice {BT_GUID = "1"};
            var bt = new Mock<IBluetooth>();

            var vm = new ItemsViewModel(Mock.Of<INavigator>(), Mock.Of<IDevicesStore>(), bt.Object, Mock.Of<ILocation>());

            // act
            vm.OnAppearing();
            bt.Raise(mock => mock.DeviceDiscovered += null, null, dev1);
            bt.Raise(mock => mock.DeviceDiscovered += null, null, dev2);
            int count = vm.AvailableDevices.Count;
            vm.OnDisappearing();

            // assert
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void AvailableDevicesList_ThrowsAwayDevicesThatAreSaved()
        {
            // arrange
            var dev1 = new BTDevice {BT_GUID = "1"};
            var dev2 = new BTDevice {BT_GUID = "1"}; // use 2 objects to verify the guid is used
            var bt = new Mock<IBluetooth>();

            var vm = new ItemsViewModel(Mock.Of<INavigator>(), Mock.Of<IDevicesStore>(), bt.Object, Mock.Of<ILocation>());

            // act
            vm.OnAppearing();
            vm.SavedDevices = new System.Collections.ObjectModel.ObservableCollection<BTDevice> { dev1 };
            bt.Raise(mock => mock.DeviceDiscovered += null, null, dev2);
            int count = vm.AvailableDevices.Count;
            vm.OnDisappearing();

            // assert
            Assert.AreEqual(0, count);
        }
    }
}
