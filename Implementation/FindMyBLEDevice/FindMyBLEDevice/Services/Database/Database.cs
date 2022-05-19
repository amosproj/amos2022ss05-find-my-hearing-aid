// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace FindMyBLEDevice.Services.Database
{
    public class Database : IDatabase
    {

        /*
         * Interface to SQLite-Database provided by the sqlite-net-pcl package.
         */
        private readonly SQLiteAsyncConnection _database;

        public Database(string path)
        {
            _database = new SQLiteAsyncConnection(path);

            // Create table if not already exists, otherwise do nothing.
            _database.CreateTableAsync<Models.BTDevice>().Wait();
        }


        public Task<List<Models.BTDevice>> GetAllDevicesAsync()
        {
            return _database.Table<Models.BTDevice>().ToListAsync();
        }

        
        public Task<Models.BTDevice> GetDeviceAsync(int id)
        {
            return _database.Table<Models.BTDevice>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        
        public Task<int> SaveDeviceAsync(Models.BTDevice device)
        {
            if (device.Id != 0)
            {
                return _database.UpdateAsync(device);
            }
            else
            {
                return _database.InsertAsync(device);
            }
        }

        
        public Task<int> DeleteDeviceAsync(Models.BTDevice device)
        {
            return _database.DeleteAsync(device);
        }

    }
}
