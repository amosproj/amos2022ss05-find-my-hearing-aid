// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FindMyBLEDevice.Tests
{
    [TestClass]
    class DatabaseTests
    {

        [TestMethod]
        public void GetAllDevicesAsync_CallsSQLitePackageMethodCorrectlyEmpty()
        {
            // arrange
            var conn = new Mock<SQLiteAsyncConnection>();
            conn.Setup(mock => mock.Table<BTDevice>().ToListAsync()).Returns(Task.FromResult(new List<BTDevice>()));
            var database = new Database(conn.Object);

            // act
            Task<List<BTDevice>> result = database.GetAllDevicesAsync();
            result.Wait();

            // assert
            conn.Verify(mock => mock.Table<BTDevice>().ToListAsync(), Times.Once);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Result.Count);

        }

        [TestMethod]
        public void GetAllDevicesAsync_CallsSQLitePackageMethodCorrectlyWithResult()
        {
            // arrange
            BTDevice a = new BTDevice()
            {
                Id = 1,
                BT_GUID = "a"
            };
            BTDevice b = new BTDevice()
            {
                Id = 2,
                BT_GUID = "b"
            };

            var conn = new Mock<SQLiteAsyncConnection>();
            conn.Setup(mock => mock.Table<BTDevice>().ToListAsync()).Returns(Task.FromResult(new List<BTDevice>() { a, b }));
            var database = new Database(conn.Object);

            // act
            Task<List<BTDevice>> result = database.GetAllDevicesAsync();
            result.Wait();

            // assert
            conn.Verify(mock => mock.Table<BTDevice>().ToListAsync(), Times.Once);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(2, result.Result.Count);
            Assert.AreEqual(1, result.Result[0].Id);
            Assert.AreEqual("a", result.Result[0].BT_GUID);
            Assert.AreEqual(2, result.Result[1].Id);
            Assert.AreEqual("b", result.Result[1].BT_GUID);

        }

        [TestMethod]
        public void GetDeviceAsync_CallsSQLitePackageMethodCorrectlyWithResult()
        {
            // arrange
            BTDevice a = new BTDevice()
            {
                Id = 1,
                BT_GUID = "a"
            };
            int id = 1;

            var conn = new Mock<SQLiteAsyncConnection>();
            conn.Setup(mock => mock.Table<BTDevice>().Where(i => i.Id == id).FirstOrDefaultAsync()).Returns(Task.FromResult(a));
            var database = new Database(conn.Object);

            // act
            Task<BTDevice> result = database.GetDeviceAsync(1);
            result.Wait();

            // assert
            conn.Verify(mock => mock.Table<BTDevice>().Where(i => i.Id == id).FirstOrDefaultAsync(), Times.Once);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(1, result.Result.Id);
            Assert.AreEqual("a", result.Result.BT_GUID);

        }

        [TestMethod]
        public void SaveDeviceAsync_Insert_CallsSQLitePackageMethodCorrectly()
        {
            // arrange
            BTDevice a = new BTDevice()
            {
                Id = 0,
                BT_GUID = "a",
                UserLabel = "userlabel",
                AdvertisedName = "advname"
            };

            var conn = new Mock<SQLiteAsyncConnection>();
            conn.Setup(mock => mock.InsertAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            conn.Setup(mock => mock.UpdateAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var database = new Database(conn.Object);

            // act
            Task<int> result = database.SaveDeviceAsync(a);
            result.Wait();

            // assert
            conn.Verify(mock => mock.InsertAsync(It.Is<BTDevice>(device =>
                device.Id == 0
                && device.BT_GUID.Equals("a")
                && device.UserLabel.Equals("userlabel")
                && device.AdvertisedName.Equals("advname"))),
                Times.Once);
            conn.Verify(mock => mock.UpdateAsync(It.Is<BTDevice>(device =>
                device.Id == 0
                && device.BT_GUID.Equals("a")
                && device.UserLabel.Equals("userlabel")
                && device.AdvertisedName.Equals("advname"))),
                Times.Never);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(1, result.Result);

        }

        [TestMethod]
        public void SaveDeviceAsync_Update_CallsSQLitePackageMethodCorrectly()
        {
            // arrange
            BTDevice a = new BTDevice()
            {
                Id = 3,
                BT_GUID = "a",
                UserLabel = "userlabel",
                AdvertisedName = "advname"
            };

            var conn = new Mock<SQLiteAsyncConnection>();
            conn.Setup(mock => mock.InsertAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            conn.Setup(mock => mock.UpdateAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var database = new Database(conn.Object);

            // act
            Task<int> result = database.SaveDeviceAsync(a);
            result.Wait();

            // assert
            conn.Verify(mock => mock.InsertAsync(It.Is<BTDevice>(device =>
                device.Id == 3
                && device.BT_GUID.Equals("a")
                && device.UserLabel.Equals("userlabel")
                && device.AdvertisedName.Equals("advname"))),
                Times.Never);
            conn.Verify(mock => mock.UpdateAsync(It.Is<BTDevice>(device =>
                device.Id == 3
                && device.BT_GUID.Equals("a")
                && device.UserLabel.Equals("userlabel")
                && device.AdvertisedName.Equals("advname"))),
                Times.Once);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(1, result.Result);

        }

        [TestMethod]
        public void DeleteDeviceAsync_CallsSQLitePackageMethodCorrectly()
        {
            // arrange
            BTDevice a = new BTDevice()
            {
                Id = 4,
                BT_GUID = "a",
                UserLabel = "userlabel",
                AdvertisedName = "advname"
            };

            var conn = new Mock<SQLiteAsyncConnection>();
            conn.Setup(mock => mock.DeleteAsync(It.IsAny<BTDevice>())).Returns(Task.FromResult(1));
            var database = new Database(conn.Object);

            // act
            Task<int> result = database.DeleteDeviceAsync(a);
            result.Wait();

            // assert
            conn.Verify(mock => mock.DeleteAsync(It.Is<BTDevice>(device =>
                device.Id == 4
                && device.BT_GUID.Equals("a")
                && device.UserLabel.Equals("userlabel")
                && device.AdvertisedName.Equals("advname"))),
                Times.Once);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(1, result.Result);

        }

    }
}
