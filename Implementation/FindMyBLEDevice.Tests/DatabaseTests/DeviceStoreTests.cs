using FindMyBLEDevice.Exceptions;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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
            const string bt_id = "some bluetooth id";
            const string name = "some name";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.AddDevice(bt_id, name);

            // assert
            db.Verify(mock => mock.SaveDeviceAsync(It.Is<BTDevice>(device =>
                device.BT_id == bt_id
                && device.Name == name
            )), Times.Once);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void AddDevice_FailureThrowsCorrectException()
        {
            // arrange
            const string bt_id = "some bluetooth id";
            const string name = "some name";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(0));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.AddDevice(bt_id, name);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is DeviceStoreException);
        }
        
        [TestMethod]
        public void UpdateDeviceName_CallsDbMethodsCorrectly()
        {
            // arrange
            const int id = 1;
            const string oldName = "some name";
            const string newName = "some other name";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(new BTDevice()
            {
                Id = id,
                Name = oldName
            }));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceName(id, newName);

            // assert
            db.Verify(mock => mock.GetDeviceAsync(It.Is<int>(arg => arg == id)), Times.Once);
            db.Verify(mock => mock.SaveDeviceAsync(It.Is<BTDevice>(device =>
            device.Id == id
            && device.Name.Equals(newName))), Times.Once);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void UpdateDeviceName_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            const string newName = "some other name";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult<BTDevice?>(null));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceName(id, newName);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            db.Verify(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>()), Times.Never);
        }

        [TestMethod]
        public void UpdateDeviceName_UpdatingFailsThrowsCorrectException()
        {
            // arrange
            const int id = 1;
            const string oldName = "some name";
            const string newName = "some other name";
            var db = new Mock<IDatabase>();
            db.Setup(mock => mock.GetDeviceAsync(It.IsAny<int>())).Returns(Task.FromResult(new BTDevice()
            {
                Id = id,
                Name = oldName
            }));
            db.Setup(mock => mock.SaveDeviceAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(0));
            var store = new DevicesStore(db.Object);

            // act
            Task result = store.UpdateDeviceName(id, newName);

            // assert
            Assert.IsTrue(result.Exception?.InnerException is DeviceStoreException);
        }
    }
}