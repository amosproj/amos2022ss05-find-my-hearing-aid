using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Tests
{
    [TestClass]
    public class DeviceStoreTests
    {
        [TestMethod]
        public void AddSingleDevice()
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
            db.Verify(mock => mock.SaveDeviceAsync(It.Is<BTDevice>(device =>
                device.BT_id == bt_id
                && device.Name == name
            )), Times.Once);
        }
    }
}