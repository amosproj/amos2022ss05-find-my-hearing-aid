// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using FindMyBLEDevice.Exceptions;
using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Tests.DevicesStoreTests
{
    [TestClass]
    public class DeviceStoreTests
    {
        [TestMethod]
        public void AddDevice_CallsDbMethodsCorrectly()
        {
            // arrange
            BTDevice device = new()
            {
                BT_GUID = "some bluetooth id",
                AdvertisedName = "some name",
                UserLabel = "some label",
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            var store = new DevicesStore(connection);

            // act
            Task<BTDevice> result = store.AddDevice(device);
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            Assert.AreEqual(device.BT_GUID, result.Result.BT_GUID);
            Assert.AreEqual(device.AdvertisedName, result.Result.AdvertisedName);
            Assert.AreEqual(device.UserLabel, result.Result.UserLabel);
            Task<BTDevice> check = connection.Table<BTDevice>()
                .Where(i => i.ID == result.Result.ID).FirstOrDefaultAsync();
            check.Wait();
            Assert.IsNull(check.Exception);
            Assert.AreEqual(device.BT_GUID, check.Result.BT_GUID);
            Assert.AreEqual(device.AdvertisedName, check.Result.AdvertisedName);
            Assert.AreEqual(device.UserLabel, check.Result.UserLabel);
            Task<int> check1 = connection.Table<BTDevice>()
                .Where(i => i.ID == result.Result.ID).CountAsync();
            check1.Wait();
            Assert.IsNull(check1.Exception);
            Assert.AreEqual(1, check1.Result);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }

        [TestMethod]
        public void UpdateDevice_CallsDbMethodsCorrectly()
        {
            // arrange
            BTDevice device = new()
            {
                ID = 1,
                BT_GUID = "some bluetooth id",
                UserLabel = "some label",
                LastGPSLatitude = 0,
                LastGPSLongitude = 0,
            };
            BTDevice newDevice = new()
            {
                ID = 1,
                BT_GUID = "some bluetooth id",
                UserLabel = "some other label",
                LastGPSLatitude = 1,
                LastGPSLongitude = 1,
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(device).Wait();
            var store = new DevicesStore(connection);

            // act
            Task result = store.UpdateDevice(newDevice);
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            Task<BTDevice> check = connection.Table<BTDevice>()
                .Where(i => i.ID == newDevice.ID).FirstOrDefaultAsync();
            check.Wait();
            Assert.IsNull(check.Exception);
            Assert.AreEqual(newDevice.UserLabel, check.Result.UserLabel);
            Assert.AreEqual(newDevice.LastGPSLatitude, check.Result.LastGPSLatitude);
            Assert.AreEqual(newDevice.LastGPSLongitude, check.Result.LastGPSLongitude);
            Task<int> check1 = connection.Table<BTDevice>()
                .Where(i => i.ID == newDevice.ID).CountAsync();
            check1.Wait();
            Assert.IsNull(check1.Exception);
            Assert.AreEqual(1, check1.Result);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }

        [TestMethod]
        public void UpdateDevice_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            BTDevice newDevice = new()
            {
                ID = 1,
                UserLabel = "some other label",
                LastGPSLatitude = 1,
                LastGPSLongitude = 1,
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            var store = new DevicesStore(connection);

            // act
            Task result = store.UpdateDevice(newDevice);
            try
            {
                result.Wait();
            }
            catch (AggregateException ae)
            {
                // assert 
                Assert.IsTrue(ae.InnerException is ArgumentException);
            }

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            Task<int> check = connection.Table<BTDevice>().CountAsync();
            check.Wait();
            Assert.AreEqual(0, check.Result);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }

        [TestMethod]
        public void DeleteDevice_CallsDbMethodsCorrectly()
        {
            // arrange
            BTDevice device = new()
            {
                ID = 1,
                BT_GUID = "some bluetooth id",
                AdvertisedName = "some name",
                UserLabel = "some label",
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(device).Wait();
            var store = new DevicesStore(connection);

            // act
            Task result = store.DeleteDevice(device.ID);
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            Task<int> check = connection.Table<BTDevice>()
                .Where(i => i.ID == device.ID).CountAsync();
            check.Wait();
            Assert.AreEqual(0, check.Result);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }

        [TestMethod]
        public void DeleteDevice_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            BTDevice device = new()
            {
                ID = 1,
                BT_GUID = "some bluetooth id",
                AdvertisedName = "some name",
                UserLabel = "some label",
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(device).Wait();
            var store = new DevicesStore(connection);

            // act
            Task result = store.DeleteDevice(device.ID + 1);
            try
            {
                result.Wait();
            }
            catch (AggregateException ae)
            {
                // assert 
                Assert.IsTrue(ae.InnerException is ArgumentException);
            }

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            Task<int> check = connection.Table<BTDevice>()
                .Where(i => i.ID == device.ID).CountAsync();
            check.Wait();
            Assert.AreEqual(1, check.Result);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }

        [TestMethod]
        public void GetDevice_CallsDbMethodsCorrectly()
        {
            // arrange
            BTDevice device = new()
            {
                BT_GUID = "some bluetooth id",
                AdvertisedName = "some name",
                UserLabel = "some label",
            };
            BTDevice otherDevice = new()
            {
                BT_GUID = "some other bluetooth id",
                AdvertisedName = "some other name",
                UserLabel = "some other label",
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(device).Wait();
            connection.InsertAsync(otherDevice).Wait();
            var store = new DevicesStore(connection);

            // act
            Task<BTDevice> result = store.GetDevice(1);
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            Assert.AreEqual(device.BT_GUID, result.Result.BT_GUID);
            Assert.AreEqual(device.AdvertisedName, result.Result.AdvertisedName);
            Assert.AreEqual(device.UserLabel, result.Result.UserLabel);
            Task<int> check1 = connection.Table<BTDevice>().CountAsync();
            check1.Wait();
            Assert.IsNull(check1.Exception);
            Assert.AreEqual(2, check1.Result);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }

        [TestMethod]
        public void GetDevice_NoDeviceWithIdThrowsCorrectException()
        {
            // arrange
            BTDevice otherDevice = new()
            {
                BT_GUID = "some other bluetooth id",
                AdvertisedName = "some other name",
                UserLabel = "some other label",
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(otherDevice).Wait();
            var store = new DevicesStore(connection);

            // act
            Task<BTDevice> result = store.GetDevice(2);
            try
            {
                result.Wait();
            }
            catch (AggregateException ae)
            {
                // assert 
                Assert.IsTrue(ae.InnerException is ArgumentException);
            }

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            Task<int> check = connection.Table<BTDevice>().CountAsync();
            check.Wait();
            Assert.AreEqual(1, check.Result);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }

        [TestMethod]
        public void GetDeviceByGuid_CallsDbMethodsCorrectly()
        {
            // arrange
            BTDevice device = new()
            {
                BT_GUID = "some bluetooth id",
                AdvertisedName = "some name",
                UserLabel = "some label",
            };
            BTDevice otherDevice = new()
            {
                BT_GUID = "some other bluetooth id",
                AdvertisedName = "some other name",
                UserLabel = "some other label",
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(device).Wait();
            connection.InsertAsync(otherDevice).Wait();
            var store = new DevicesStore(connection);

            // act
            Task<BTDevice> result = store.GetDeviceByGUID(device.BT_GUID);
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            Assert.AreEqual(device.BT_GUID, result.Result.BT_GUID);
            Assert.AreEqual(device.AdvertisedName, result.Result.AdvertisedName);
            Assert.AreEqual(device.UserLabel, result.Result.UserLabel);
            Task<int> check1 = connection.Table<BTDevice>().CountAsync();
            check1.Wait();
            Assert.IsNull(check1.Exception);
            Assert.AreEqual(2, check1.Result);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }

        [TestMethod]
        public void GetDeviceByGuid_NoDeviceWithGuidThrowsCorrectException()
        {
            // arrange
            BTDevice otherDevice = new()
            {
                BT_GUID = "some other bluetooth id",
                AdvertisedName = "some other name",
                UserLabel = "some other label",
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(otherDevice).Wait();
            var store = new DevicesStore(connection);

            // act
            Task<BTDevice> result = store.GetDeviceByGUID("some bluetooth id");
            try
            {
                result.Wait();
            }
            catch (AggregateException ae)
            {
                // assert 
                Assert.IsTrue(ae.InnerException is ArgumentException);
            }

            // assert
            Assert.IsTrue(result.Exception?.InnerException is ArgumentException);
            Task<int> check = connection.Table<BTDevice>().CountAsync();
            check.Wait();
            Assert.AreEqual(1, check.Result);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }

        [TestMethod]
        public void GetAllDevices_CallsDbMethodCorrectly()
        {
            // arrange
            BTDevice device = new()
            {
                BT_GUID = "some bluetooth id",
                AdvertisedName = "some name",
                UserLabel = "some label",
            };
            BTDevice otherDevice = new()
            {
                BT_GUID = "some other bluetooth id",
                AdvertisedName = "some other name",
                UserLabel = "some other label",
            };
            SQLiteAsyncConnection connection = new(":memory:");
            connection.CreateTableAsync<Models.BTDevice>().Wait();
            connection.InsertAsync(device).Wait();
            connection.InsertAsync(otherDevice).Wait();
            var store = new DevicesStore(connection);

            // act 
            Task<List<BTDevice>> result = store.GetAllDevices();
            result.Wait();

            // assert
            Assert.IsNull(result.Exception);
            Assert.AreEqual(2, result.Result.Count);
            Assert.AreEqual(1, result.Result.FindAll(i => 
                i.BT_GUID == device.BT_GUID && 
                i.AdvertisedName == device.AdvertisedName && 
                i.UserLabel == device.UserLabel).Count);
            Assert.AreEqual(1, result.Result.FindAll(i =>
                i.BT_GUID == device.BT_GUID &&
                i.AdvertisedName == device.AdvertisedName &&
                i.UserLabel == device.UserLabel).Count);

            // clean-up
            connection.DropTableAsync<BTDevice>().Wait();
        }
    }
}
