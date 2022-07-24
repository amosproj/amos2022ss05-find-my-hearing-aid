// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Geolocation;
using FindMyBLEDevice.ViewModels;
using FindMyBLEDevice.XamarinAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class MapViewModelTests
    {
        [TestMethod]
        public void DisplaysCorrectDevicePinOnAppearing()
        {
            // arrange
            var datetime = DateTime.Now;
            var label = "userlabel";
            var longitude = 0;
            var latitude = 0;
            var dev = new BTDevice
            {
                UserLabel = label,
                LastGPSLatitude = latitude,
                LastGPSLongitude = longitude,
                LastGPSTimestamp = datetime
            };
            var map = new Map();
            var ds = new Mock<IDevicesStore>();
            ds.SetupGet(mock => mock.SelectedDevice).Returns(dev);
            var vm = new MapViewModel(map, Mock.Of<IGeolocation>(), Mock.Of<INavigator>(), ds.Object, Mock.Of<IDeviceAccess>());

            // act
            vm.OnAppearing();

            //assert
            Assert.AreEqual(1, map.Pins.Count);
            Assert.AreEqual(label, map.Pins[0].Label);
            Assert.AreEqual(datetime.ToString(), map.Pins[0].Address);
            Assert.AreEqual(PinType.Place, map.Pins[0].Type);
            Assert.AreEqual(longitude, map.Pins[0].Position.Longitude);
            Assert.AreEqual(latitude, map.Pins[0].Position.Latitude);
        }

        [TestMethod]
        public void ChangedDeviceUpdatesPin()
        {
            // arrange
            const string newLabel = "newlabel";
            var device = new BTDevice() { ID = 0, UserLabel = "this should not be present after the event was raised" };
            var map = new Map();
            int handlersRegistered = 0;
            int handlersUnregistered = 0;

            var ds = new Mock<IDevicesStore>();
            ds.Setup(mock => mock.GetDevice(device.ID)).Returns(Task.FromResult(new BTDevice() { UserLabel = newLabel }));
            ds.SetupGet(mock => mock.SelectedDevice).Returns(device);
            ds.SetupAdd(mock => mock.DevicesChanged += It.IsAny<EventHandler<List<int>>>()).Callback(() => handlersRegistered++);
            ds.SetupRemove(mock => mock.DevicesChanged -= It.IsAny<EventHandler<List<int>>>()).Callback(() => handlersUnregistered++);
            var vm = new MapViewModel(map, Mock.Of<IGeolocation>(), Mock.Of<INavigator>(), ds.Object, Mock.Of<IDeviceAccess>());

            // act
            vm.OnAppearing();
            ds.Raise(mock => mock.DevicesChanged += null, null, new List<int>(){ device.ID });
            vm.OnDisappearing();

            // assert
            Assert.AreEqual(handlersRegistered, handlersUnregistered);
            Assert.AreEqual(1, map.Pins.Count);
            Assert.AreEqual(newLabel, map.Pins[0].Label);
        }

        [TestMethod]
        public void DeletingDeviceRemovesPin()
        {
            // arrange
            var device = new BTDevice() { ID = 0, UserLabel = "this should not be present after the event was raised" };
            var map = new Map();

            var ds = new Mock<IDevicesStore>();
            BTDevice nullDev = null;
            ds.SetupSequence(mock => mock.SelectedDevice).Returns(device).Returns(nullDev);
            var vm = new MapViewModel(map, Mock.Of<IGeolocation>(), Mock.Of<INavigator>(), ds.Object, Mock.Of<IDeviceAccess>());

            // act
            vm.OnAppearing();
            ds.Raise(mock => mock.DevicesChanged += null, null, new List<int>(){ device.ID });
            vm.OnDisappearing();

            // assert
            Assert.AreEqual(0, map.Pins.Count);
        }

        [TestMethod]
        public void CurrentDeviceReachableCausesDisplayAlert()
        {
            // arrange
            var device = new BTDevice() { ID = 0, WithinRange = true };
            var map = new Map();

            var ds = new Mock<IDevicesStore>();
            ds.SetupGet(mock => mock.SelectedDevice).Returns(device);
            ds.Setup(mock => mock.GetDevice(device.ID)).Returns(Task.FromResult(device));
            var da = new Mock<IDeviceAccess>();
            var vm = new MapViewModel(map, Mock.Of<IGeolocation>(), Mock.Of<INavigator>(), ds.Object, da.Object);

            // act
            vm.OnAppearing();
            // raise it twice to verify the handler unregisters itself
            ds.Raise(mock => mock.DevicesChanged += null, null, new List<int>(){ device.ID });
            ds.Raise(mock => mock.DevicesChanged += null, null, new List<int>(){ device.ID });
            vm.OnDisappearing();

            // assert
            da.Verify(mock => mock.InvokeOnMainThreadAsync(It.IsAny<Func<Task>>()), Times.Once);
        }
    }
}
