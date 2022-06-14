// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.ObjectModel;
using System.Threading;
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
            Assert.AreEqual(id.ToString(), available[0].BT_GUID);
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

        [TestMethod]
        public async Task StopSearch_MakesCorrectAdapterCall()
        {
            // arrange
            var adapter = new Mock<IAdapter>();
            adapter.SetupGet(mock => mock.IsScanning).Returns(true);
            var bt = new Bluetooth(adapter.Object);

            // act
            await bt.StopSearch();

            // assert
            adapter.VerifyGet(mock => mock.IsScanning, Times.Once);
            adapter.Verify(mock => mock.StopScanningForDevicesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task StartAndStopRssiPolling()
        {
            // arrange
            const int fakeRssi = 1;
            var device = new Mock<IDevice>();
            device.SetupGet(mock => mock.Rssi).Returns(fakeRssi);
            device.SetupGet(mock => mock.State).Returns(DeviceState.Connected);
            var adapter = new Mock<IAdapter>();
            adapter
                .Setup(mock => mock.ConnectToKnownDeviceAsync(It.IsAny<Guid>(), It.IsAny<ConnectParameters>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(device.Object));
            var bt = new Bluetooth(adapter.Object);

            // act
            int rssi = 0;
            bt.StartRssiPolling(Guid.Empty.ToString(), (int input) =>
            {
                rssi = input;
            },
            () => { },
            () => { }
            );
            Thread.Sleep(100); // polling interval is 25ms
            bt.StopRssiPolling();

            // assert
            device.Verify(mock => mock.UpdateRssiAsync(), Times.Between(1, 4, Moq.Range.Inclusive));
            device.VerifyGet(mock => mock.Rssi, Times.Between(1, 4, Moq.Range.Inclusive));
            Assert.AreEqual(fakeRssi, rssi);
        }

        [TestMethod]
        public void StopRssiPolling_WithoutStartDoesNotBreakAnything()
        {
            // arrange
            var bt = new Bluetooth(null);
            Exception? exception = null;

            // act
            try
            {
                bt.StopRssiPolling();
            } catch (Exception e) {
                exception = e;
            }

            // assert
            Assert.IsNull(exception);
        }
    }
}
