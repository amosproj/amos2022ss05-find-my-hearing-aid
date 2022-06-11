// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using System.Collections.Generic;
using System.Threading.Tasks;
using FindMyBLEDevice.Models;
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
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }


        public Task<Models.BTDevice> GetDeviceByGUIDAsync(string guid)
        {
            return _database.Table<Models.BTDevice>()
                            .Where(i => i.BT_GUID.CompareTo(guid) == 0)
                            .FirstOrDefaultAsync();
        }


        public Task<int> SaveDeviceAsync(Models.BTDevice device)
        {
            if (device.ID != 0)
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
