// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Geolocation;
using FindMyBLEDevice.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Xamarin.Forms.Maps;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class MapViewModelTests
    {
        [TestMethod]
        public void DisplaysDevicePinOnAppearing()
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
            var vm = new MapViewModel(map, Mock.Of<IGeolocation>(), Mock.Of<INavigator>(), ds.Object);

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
    }
}
