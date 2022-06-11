// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Tests.BluetoothTests
{
    [TestClass]
    public class BluetoothTests
    {
        [TestMethod]
        public async Task Search_EliminatesDuplicates()
        {
            // arrange
            Guid id = Guid.Empty;
            const string name = "some name";
            const int rssi = 0;
            var device = new Mock<IDevice>();
            device.SetupGet(mock => mock.Id).Returns(id);
            device.SetupGet(mock => mock.Name).Returns(name);
            device.SetupGet(mock => mock.Rssi).Returns(rssi);
            DeviceEventArgs args = new DeviceEventArgs();
            args.Device = device.Object;

            var adapter = new Mock<IAdapter>();
            var bt = new Bluetooth(adapter.Object);

            ObservableCollection<BTDevice> available = new ObservableCollection<BTDevice>();

            // act
            await bt.Search(100, available, null);
            for (int i = 0; i < 3; i++)
            {
                adapter.Raise(mock => mock.DeviceDiscovered += null, args);
            }

            // assert
            Assert.AreEqual(1, available.Count);
            Assert.AreEqual(id, available[0].BT_GUID);
        }

        [TestMethod]
        public async Task Search_IgnoresDevicesTooFarAway()
        {
            // arrange
            Guid id = Guid.Empty;
            const string name = "some name";
            const int rssi = -100;
            var device = new Mock<IDevice>();
            device.SetupGet(mock => mock.Id).Returns(id);
            device.SetupGet(mock => mock.Name).Returns(name);
            device.SetupGet(mock => mock.Rssi).Returns(rssi);
            DeviceEventArgs args = new DeviceEventArgs();
            args.Device = device.Object;

            var adapter = new Mock<IAdapter>();
            var bt = new Bluetooth(adapter.Object);

            ObservableCollection<BTDevice> available = new ObservableCollection<BTDevice>();

            // act
            await bt.Search(100, available, null);
            adapter.Raise(mock => mock.DeviceDiscovered += null, args);

            // assert
            Assert.AreEqual(0, available.Count);
        }

        [TestMethod]
        public async Task Search_IgnoresDevicesWithoutName()
        {
            // arrange
            Guid id = Guid.Empty;
            const int rssi = 0;
            var device = new Mock<IDevice>();
            device.SetupGet(mock => mock.Id).Returns(id);
            device.SetupGet(mock => mock.Rssi).Returns(rssi);
            DeviceEventArgs args = new DeviceEventArgs();
            args.Device = device.Object;

            var adapter = new Mock<IAdapter>();
            var bt = new Bluetooth(adapter.Object);

            ObservableCollection<BTDevice> available = new ObservableCollection<BTDevice>();

            // act
            await bt.Search(100, available, null);
            adapter.Raise(mock => mock.DeviceDiscovered += null, args);

            // assert
            Assert.AreEqual(0, available.Count);
        }

        [TestMethod]
        public async Task Search_IgnoresDevicesOnFilter()
        {
            // arrange
            Guid id = Guid.Empty;
            const int rssi = 0;
            var device = new Mock<IDevice>();
            device.SetupGet(mock => mock.Id).Returns(id);
            device.SetupGet(mock => mock.Rssi).Returns(rssi);
            DeviceEventArgs args = new DeviceEventArgs();
            args.Device = device.Object;

            var adapter = new Mock<IAdapter>();
            var bt = new Bluetooth(adapter.Object);

            ObservableCollection<BTDevice> available = new ObservableCollection<BTDevice>();

            // act
            await bt.Search(100, available, o => false);
            adapter.Raise(mock => mock.DeviceDiscovered += null, args);

            // assert
            Assert.AreEqual(0, available.Count);
        }
    }
}
