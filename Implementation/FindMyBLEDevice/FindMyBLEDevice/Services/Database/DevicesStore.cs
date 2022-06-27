// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using FindMyBLEDevice.Exceptions;
using FindMyBLEDevice.Models;

namespace FindMyBLEDevice.Services.Database
{
    public class DevicesStore : IDevicesStore
    {

        private readonly IDatabase _database;

        public event EventHandler DevicesChanged;

        public BTDevice SelectedDevice { get; set; }

        public DevicesStore()
        {
            _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BTDevices.db3"));
        }

        public DevicesStore(IDatabase database)
        {
            _database = database;
        }


        public async Task<BTDevice> AddDevice(Models.BTDevice device)
        {
            Console.WriteLine("DevicesStore.AddDevice:");
            Console.WriteLine(device.BT_GUID);
            Console.WriteLine(device.AdvertisedName);
            Console.WriteLine(device.UserLabel);

            device.ID = 0; //so that database adds instead of overwriting existing device
            int result = await _database.SaveDeviceAsync(device);

            if(result != 1)
            {
                throw new DeviceStoreException("Saving device failed!");
            }

            DevicesChanged?.Invoke(this, EventArgs.Empty);

            return await GetDeviceByGUID(device.BT_GUID);
        }

        public async Task UpdateDevice(Models.BTDevice device)
        {
            Models.BTDevice storedDevice = await _database.GetDeviceAsync(device.ID);

            if (storedDevice is null)
            {
                throw new ArgumentException("Device with given id is not saved in database!");
            }

            int result = await _database.SaveDeviceAsync(device);

            if (result != 1)
            {
                throw new DeviceStoreException("Updating device failed!");
            }

            DevicesChanged?.Invoke(this, EventArgs.Empty);

        }

        public async Task DeleteDevice(int id)
        {
            Models.BTDevice device = await _database.GetDeviceAsync(id);

            if (device is null)
            {
                throw new ArgumentException("Device with given id is not saved in database!");
            }

            int result = await _database.DeleteDeviceAsync(device);

            if (result != 1)
            {
                throw new DeviceStoreException("Updating device failed!");
            }

            DevicesChanged?.Invoke(this, EventArgs.Empty);

        }

        public async Task<Models.BTDevice> GetDevice(int id)
        {

            Models.BTDevice device = await _database.GetDeviceAsync(id);
            if (device is null)
            {
                throw new ArgumentException("Device with given id is not saved in database!");
            }

            return device;
        }

        public async Task<BTDevice> GetDeviceByGUID(string guid)
        {
            Models.BTDevice device = await _database.GetDeviceByGUIDAsync(guid);
            if (device is null)
            {
                throw new ArgumentException("Device with given guid is not saved in database!");
            }

            return device;
        }

        public async Task<List<Models.BTDevice>> GetAllDevices()
        {
            return await _database.GetAllDevicesAsync();
        }

    }

}
