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
            const string bt_guid = "some bluetooth id";
            const string advertisedName = "some name";
            const string userLabel = "some label";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.AddDevice(bt_guid, advertisedName, userLabel);

            // assert
            db.Verify(mock => mock.SaveDeviceAsync(It.Is<BTDevice>(device =>
                device.BT_GUID == bt_guid
                && device.AdvertisedName == advertisedName
                && device.UserLabel == userLabel
                )), Times.Once);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void AddDevice_FailureThrowsCorrectException()
        {
            // arrange
            const string bt_guid = "some bluetooth id";
            const string advertisedName = "some name";
            const string userLabel = "some label";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(0));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.AddDevice(bt_guid, advertisedName, userLabel);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is DeviceStoreException);
        }
        
        [TestMethod]
        public void UpdateDeviceUserLabel_CallsDbMethodsCorrectly()
        {
            // arrange
            const int id = 1;
            const string oldLabel = "some label";
            const string newLabel = "some other label";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(new BTDevice()
            {
                Id = id,
                UserLabel = oldLabel
            }));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceUserLabel(id, newLabel);

            // assert
            db.Verify(mock => mock.GetDeviceAsync(It.Is<int>(arg => arg == id)), Times.Once);
            db.Verify(mock => mock.SaveDeviceAsync(It.Is<BTDevice>(device =>
                device.Id == id
                && device.UserLabel.Equals(newLabel))),
                Times.Once);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void UpdateDeviceUserLabel_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            const string newLabel = "some other label";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice?>(null));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceUserLabel(id, newLabel);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            db.Verify(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>()), Times.Never);
        }

        [TestMethod]
        public void UpdateDeviceUserLabel_UpdatingFailsThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            const string oldLabel = "some label";
            const string newLabel = "some other label";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(new BTDevice()
            {
                Id = id,
                UserLabel = oldLabel
            }));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(0));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceUserLabel(id, newLabel);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is DeviceStoreException);
        }

        [TestMethod]
        public void UpdateDeviceLocation_CallsDbMethodsCorrectly()
        {
            // arrange
            const int id = 1;
            const int oldVals = 0;
            const int newVals = 1;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(new BTDevice()
            {
                Id = id,
                LastGPSLatitude = oldVals,
                LastGPSLongitude = oldVals
            }));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceLocation(id, newVals, newVals);

            // assert
            db.Verify(mock => mock.GetDeviceAsync(It.Is<int>(arg => arg == id)), Times.Once);
            db.Verify(mock => mock.SaveDeviceAsync(It.Is<BTDevice>(device =>
                device.Id == id
                && device.LastGPSLongitude.Equals(newVals)
                && device.LastGPSLatitude.Equals(newVals)
                && (device.LastGPSTimestamp - DateTime.Now).TotalMilliseconds < 5)),
                Times.Once);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void UpdateDeviceLocation_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            const int newVals = 1;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice?>(null));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceLocation(id, newVals, newVals);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            db.Verify(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>()), Times.Never);
        }

        [TestMethod]
        public void UpdateDeviceLocation_UpdatingFailsThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            const int oldVals = 0;
            const int newVals = 1;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(new BTDevice()
            {
                Id = id,
                LastGPSLatitude = oldVals,
                LastGPSLongitude = oldVals
            }));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(0));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceLocation(id, newVals, newVals);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is DeviceStoreException);
        }

        [TestMethod]
        public void UpdateDeviceActive_CallsDbMethodsCorrectly()
        {
            // arrange
            const int id = 1;
            const bool oldActive = false;
            const bool newActive = true;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(new BTDevice()
            {
                Id = id,
                Active = oldActive
            }));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceActive(id, newActive);

            // assert
            db.Verify(mock => mock.GetDeviceAsync(It.Is<int>(arg => arg == id)), Times.Once);
            db.Verify(mock => mock.SaveDeviceAsync(It.Is<BTDevice>(device =>
                device.Id == id
                && device.Active.Equals(newActive))),
                Times.Once);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void UpdateDeviceActive_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            const bool newActive = true;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice?>(null));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceActive(id, newActive);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            db.Verify(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>()), Times.Never);
        }

        [TestMethod]
        public void UpdateDeviceActive_UpdatingFailsThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            const bool oldActive = false;
            const bool newActive = true;
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(new BTDevice()
            {
                Id = id,
                Active = oldActive
            }));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(0));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceActive(id, newActive);

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
                Id = id
            }));
            db.Setup(mock => mock.DeleteDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.DeleteDevice(id);

            // assert
            db.Verify(mock => mock.GetDeviceAsync(It.Is<int>(arg => arg == id)), Times.Once);
            db.Verify(mock => mock.DeleteDeviceAsync(It.Is<BTDevice>(device =>
                device.Id == id)),
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
                Id = id
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
                Id = id
            }));
            var store = new DevicesStore(db.Object);

            // act
            Task<BTDevice> result = store.GetDevice(id);
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            db.Verify(mock => mock.GetDeviceAsync(It.Is<int>(arg => arg == id)), Times.Once);
            Assert.AreEqual(id, result.Result.Id);
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