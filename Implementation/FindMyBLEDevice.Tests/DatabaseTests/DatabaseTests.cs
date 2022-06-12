// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FindMyBLEDevice.Tests
{

    [TestClass]
    public class DatabaseTests
    {

        [TestMethod]
        public void GetAllDevicesAsync_CallsSQLitePackageMethodCorrectlyEmpty()
        {
            // arrange
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();            
            var database = new Database(connection);

            // act
            Task<List<BTDevice>> result = database.GetAllDevicesAsync();
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Result.Count);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();

        }

        [TestMethod]
        public void GetAllDevicesAsync_CallsSQLitePackageMethodCorrectlyWithResult()
        {
            // arrange
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID1",
                AdvertisedName = "AdvName1"
            }).Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID2",
                AdvertisedName = "AdvName2"
            }).Wait();
            var database = new Database(connection);

            // act
            Task<List<BTDevice>> result = database.GetAllDevicesAsync();
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            Assert.AreEqual(2, result.Result.Count);
            Assert.AreEqual("BTID1", result.Result[0].BT_GUID);
            Assert.AreEqual("AdvName1", result.Result[0].AdvertisedName);
            Assert.AreEqual("BTID2", result.Result[1].BT_GUID);
            Assert.AreEqual("AdvName2", result.Result[1].AdvertisedName);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();

        }

        [TestMethod]
        public void GetDeviceAsync_CallsSQLitePackageMethodCorrectlyWithResult()
        {
            // arrange
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID1",
                AdvertisedName = "AdvName1"
            }).Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID2",
                AdvertisedName = "AdvName2"
            }).Wait();
            var database = new Database(connection);

            // act
            Task<BTDevice> result = database.GetDeviceAsync(1);
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            Assert.AreEqual(1, result.Result.Id);
            Assert.AreEqual("BTID1", result.Result.BT_GUID);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();

        }

        [TestMethod]
        public void GetDeviceAsync_CallsSQLitePackageMethodCorrectlyWithoutResult()
        {
            // arrange
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID1",
                AdvertisedName = "AdvName1"
            }).Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID2",
                AdvertisedName = "AdvName2"
            }).Wait();
            var database = new Database(connection);

            // act
            Task<BTDevice> result = database.GetDeviceAsync(4);
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();

        }

        [TestMethod]
        public void SaveDeviceAsync_Insert_CallsSQLitePackageMethodCorrectly()
        {
            // arrange
            BTDevice a = new BTDevice()
            {
                Id = 0,
                BT_GUID = "NEWBTGUID",
                UserLabel = "userlabel",
                AdvertisedName = "advname"
            };

            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID1",
                AdvertisedName = "AdvName1"
            }).Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID2",
                AdvertisedName = "AdvName2"
            }).Wait();
            var database = new Database(connection);

            // act
            Task<int> result = database.SaveDeviceAsync(a);
            result.Wait();

            // assert            
            Assert.IsNull(result.Exception);
            Assert.AreEqual(1, result.Result);
            Task<BTDevice> check = database.GetDeviceAsync(3);
            check.Wait();
            Assert.AreEqual(3, check.Result.Id);
            Assert.AreEqual("NEWBTGUID", check.Result.BT_GUID);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();

        }

        [TestMethod]
        public void SaveDeviceAsync_Update_CallsSQLitePackageMethodCorrectly()
        {
            // arrange
            BTDevice a = new BTDevice()
            {
                Id = 1,
                BT_GUID = "BTID1_UPDATED",
                UserLabel = "userlabel",
                AdvertisedName = "AdvName1Updated"
            };
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID1",
                AdvertisedName = "AdvName1"
            }).Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID2",
                AdvertisedName = "AdvName2"
            }).Wait();
            var database = new Database(connection);

            // act
            Task<int> result = database.SaveDeviceAsync(a);
            result.Wait();

            // assert            
            Assert.IsNull(result.Exception);
            Assert.AreEqual(1, result.Result);
            Task<BTDevice> check = database.GetDeviceAsync(1);
            check.Wait();
            Assert.AreEqual(1, check.Result.Id);
            Assert.AreEqual("BTID1_UPDATED", check.Result.BT_GUID);
            Assert.AreEqual("userlabel", check.Result.UserLabel);
            Assert.AreEqual("AdvName1Updated", check.Result.AdvertisedName);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();

        }

        [TestMethod]
        public void DeleteDeviceAsync_CallsSQLitePackageMethodCorrectly()
        {
            // arrange
            BTDevice a = new BTDevice()
            {
                Id = 1,
                BT_GUID = "BTID1",
                AdvertisedName = "AdvName1"
            };
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID1",
                AdvertisedName = "AdvName1"
            }).Wait();
            connection.InsertAsync(new BTDevice
            {
                BT_GUID = "BTID2",
                AdvertisedName = "AdvName2"
            }).Wait();
            var database = new Database(connection);

            // act
            Task<int> result = database.DeleteDeviceAsync(a);
            result.Wait();

            // assert            
            Assert.IsNull(result.Exception);
            Assert.AreEqual(1, result.Result);
            Task<BTDevice> check = database.GetDeviceAsync(1);
            check.Wait();
            Assert.IsNull(check.Result);
            Task<List<BTDevice>> check2 = database.GetAllDevicesAsync();
            check2.Wait();
            Assert.AreEqual(1, check2.Result.Count);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();

        }

    }
}
