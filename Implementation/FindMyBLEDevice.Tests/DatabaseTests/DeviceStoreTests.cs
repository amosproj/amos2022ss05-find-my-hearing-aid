// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Exceptions;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Tests
{
    [TestClass]
    public class DeviceStoreTests
    {
        [TestMethod]
        public void AddDevice_CallsDbMethodCorrectly()
        {
            // arrange
            BTDevice device = new()
            {
                BT_GUID = "some bluetooth id",
                AdvertisedName = "some name",
                UserLabel = "some label",
            };
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.AddDevice(device);

            // assert
            db.Verify(mock => mock.SaveDeviceAsync(It.Is<BTDevice>(savedDevice =>
                device.BT_GUID == savedDevice.BT_GUID
                && device.AdvertisedName == savedDevice.AdvertisedName
                && device.UserLabel == savedDevice.UserLabel
                )), Times.Once);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void AddDevice_FailureThrowsCorrectException()
        {
            // arrange
            BTDevice device = new()
            {
                BT_GUID = "some bluetooth id",
                AdvertisedName = "some name",
                UserLabel = "some label",
            };
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(0));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.AddDevice(device);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is DeviceStoreException);
        }
        
        [TestMethod]
        public void UpdateDevice_CallsDbMethodsCorrectly()
        {
            // arrange
            BTDevice device = new()
            {
                ID = 1,
                UserLabel = "some label",
                LastGPSLatitude = 0,
                LastGPSLongitude = 0,
            };
            BTDevice newDevice = new()
            {
                ID = 1,
                UserLabel = "some other label",
                LastGPSLatitude = 1,
                LastGPSLongitude = 1,
            };
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(device));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDevice(newDevice);

            // assert
            db.Verify(mock => mock.GetDeviceAsync(It.Is<int>(arg => arg == device.ID)), Times.Once);
            db.Verify(mock => mock.SaveDeviceAsync(It.Is<BTDevice>(savedDevice =>
                savedDevice.ID == device.ID
                && savedDevice.UserLabel.Equals(newDevice.UserLabel)
                && device.LastGPSLongitude.Equals(newDevice.LastGPSLongitude)
                && device.LastGPSLatitude.Equals(newDevice.LastGPSLatitude))),
                Times.Once);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void UpdateDevice_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            BTDevice newDevice = new()
            {
                ID = 1,
                UserLabel = "some other label",
            };
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice?>(null));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDevice(newDevice);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            db.Verify(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>()), Times.Never);
        }

        [TestMethod]
        public void UpdateDevice_UpdatingFailsThrowsCorrectException()
        {
            // arrange
            BTDevice device = new()
            {
                ID = 1,
                UserLabel = "some label",
            };
            BTDevice newDevice = new()
            {
                ID = 1,
                UserLabel = "some other label",
            };
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(device));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(0));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDevice(newDevice);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is DeviceStoreException);
        }

        [TestMethod]
        public void DeleteDevice_CallsDbMethodsCorrectly()
        {
            // arrange
            const int id = 1;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice?>(new BTDevice()
            {
                ID = id
            }));
            db.Setup(mock => mock.DeleteDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.DeleteDevice(id);

            // assert
            db.Verify(mock => mock.GetDeviceAsync(It.Is<int>(arg => arg == id)), Times.Once);
            db.Verify(mock => mock.DeleteDeviceAsync(It.Is<BTDevice>(device =>
                device.ID == id)),
                Times.Once);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void DeleteDevice_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice?>(null));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.DeleteDevice(id);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            db.Verify(mock => mock.DeleteDeviceAsync(It.IsAny<BTDevice>()), Times.Never);
        }

        [TestMethod]
        public void DeleteDevice_DeletingFailsThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice?>(new BTDevice()
            {
                ID = id
            }));
            db.Setup(mock => mock.DeleteDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(0));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.DeleteDevice(id);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is DeviceStoreException);
        }

        [TestMethod]
        public void GetDevice_CallsDbMethodsCorrectly()
        {
            // arrange
            const int id = 1;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice>(new BTDevice()
            {
                ID = id
            }));
            var store = new DevicesStore(db.Object);

            // act
            Task<BTDevice> result = store.GetDevice(id);
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            db.Verify(mock => mock.GetDeviceAsync(It.Is<int>(arg => arg == id)), Times.Once);
            Assert.AreEqual(id, result.Result.ID);
        }

        [TestMethod]
        public void GetDevice_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice?>(null));
            var store = new DevicesStore(db.Object);

            // act
            Task<BTDevice> result = store.GetDevice(id);
            try
            {
                result.Wait();
            } catch (AggregateException ae)
            {
                // assert 
                Assert.IsTrue(ae.InnerException is ArgumentException);
            }
        }

        [TestMethod]
        public void GetAllDevices_CallsDbMethodCorrectly()
        {
            // arrange
            var db = new Mock<IDatabase>();
            var store = new DevicesStore(db.Object);
            
            // act 
            Task<List<BTDevice>> result = store.GetAllDevices();

            // assert
            Assert.IsNull(result.Exception);
            db.Verify(mock => mock.GetAllDevicesAsync(), Times.Once);
        }
    }
}