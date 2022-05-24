// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Tests.DatabaseTests
{
    [TestClass]
    public class DatabaseComponentTests
    {
        private static string DbPath;
        private static DevicesStore Store;

        [ClassInitialize]
        public static void Init()
        {
            DbPath = Path.Combine(Path.GetTempPath(), "testdb.db3");
            if (File.Exists(DbPath))
            {
                File.Delete(DbPath);
            }
            Store = new DevicesStore(new Database(DbPath));
        }

        [ClassCleanup]
        public static void Destroy()
        {
            File.Delete(DbPath);
        }

        [TestInitialize]
        public void InitNextTest()
        {
            // get all entries of test db
            Task<List<BTDevice>> getTask = Store.GetAllDevices();
            try
            {
                getTask.Wait();
            } catch (AggregateException ae)
            {
                Console.WriteLine(ae.InnerException);
            }
            List<BTDevice> devices = getTask.Result;

            // delete them
            foreach (BTDevice device in devices)
            {
                Task delTask = Store.DeleteDevice(device.Id);
                try
                {
                    delTask.Wait();
                } catch (AggregateException ae)
                {
                    Console.WriteLine(ae.InnerException);
                }
            }
        }

        [TestMethod]
        public void AddSingleDevice()
        {
            // arrange
            const string bt_id = "some id";
            const string name = "some name";

            // act
            try
            {
                Task addTask = Store.AddDevice(bt_id, name);
                addTask.Wait();
                Task<List<BTDevice>> getAllTask = Store.GetAllDevices();
                getAllTask.Wait();
                List<BTDevice> devices = getAllTask.Result;
            } catch (AggregateException ae)
            {
                Console.WriteLine(ae.InnerException);
            }
        }
    }
}
