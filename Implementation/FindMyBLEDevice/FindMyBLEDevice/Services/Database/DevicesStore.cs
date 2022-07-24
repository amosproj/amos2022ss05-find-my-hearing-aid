// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using FindMyBLEDevice.Exceptions;
using FindMyBLEDevice.Models;
using SQLite;

namespace FindMyBLEDevice.Services.Database
{
    public class DevicesStore : IDevicesStore
    {
        /*
         * Interface to SQLite-Database provided by the sqlite-net-pcl package.
         */
        private readonly SQLiteAsyncConnection _database;

        public event EventHandler<List<int>> DevicesChanged;

        public BTDevice SelectedDevice { get; set; }


        public DevicesStore(SQLiteAsyncConnection database)
        {
            _database = database;

            // Create table if not already exists, otherwise do nothing.
            _database.CreateTableAsync<BTDevice>().Wait();
        }
        public DevicesStore() : this(new SQLiteAsyncConnection(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BTDevices.db3"))) {}


        public async Task<BTDevice> AddDevice(BTDevice device)
        {
            device.ID = 0; //so that database adds instead of overwriting existing device
            device.CreatedAt = DateTime.Now;
            int result = await _database.InsertAsync(device);

            if (result != 1)
            {
                throw new DeviceStoreException("Saving device failed!");
            }

            DevicesChanged?.Invoke(this, new List<int>() { device.ID });

            return await GetDeviceByGUID(device.BT_GUID);
        }

        public async Task UpdateDevice(BTDevice device)
        {
            _ = await GetDevice(device.ID); //checks wheter device exists in database

            int result = await _database.UpdateAsync(device);

            if (result != 1)
            {
                throw new DeviceStoreException("Updating device failed!");
            }

            DevicesChanged?.Invoke(this, new List<int>() { device.ID });

        }

        public async Task DeleteDevice(int id)
        {
            BTDevice device = await GetDevice(id); //checks wheter device exists in database

            int result = await _database.DeleteAsync(device);

            if (result != 1)
            {
                throw new DeviceStoreException("Deleting device failed!");
            }

            DevicesChanged?.Invoke(this, new List<int>() { id });

        }

        public async Task<BTDevice> GetDevice(int id)
        {

            BTDevice device = await _database.Table<BTDevice>()
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();

            if (device is null)
            {
                throw new ArgumentException("Device with given id is not saved in database!");
            }

            return device;
        }

        public async Task<BTDevice> GetDeviceByGUID(string guid)
        {
            BTDevice device = await _database.Table<Models.BTDevice>()
                .Where(i => i.BT_GUID.Equals(guid))
                .FirstOrDefaultAsync();

            if (device is null)
            {
                throw new ArgumentException("Device with given guid is not saved in database!");
            }

            return device;
        }

        public Task<List<BTDevice>> GetAllDevices()
        {
            return _database.Table<BTDevice>().ToListAsync();
        }

        public Task AtomicGetAndUpdateDevice(int id, Action<BTDevice> manipulation)
        {
            return Task.Run(() =>
            {
                lock (_database)
                {
                    var getTask = GetDevice(id);
                    getTask.Wait();
                    BTDevice latestVersion = getTask.Result;
                    manipulation?.Invoke(latestVersion);
                    var updateTask = UpdateDevice(latestVersion);
                    updateTask.Wait();
                }
            });
        }
    }
}
