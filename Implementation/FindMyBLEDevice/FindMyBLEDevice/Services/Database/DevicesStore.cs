using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using FindMyBLEDevice.Exceptions;

namespace FindMyBLEDevice.Services.Database
{
    public class DevicesStore : IDevicesStore
    {

        private readonly IDatabase _database;

        public DevicesStore()
        {
            _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BTDevices.db3"));
        }

        public DevicesStore(IDatabase database)
        {
            _database = database;
        }


        public async Task AddDevice(string bt_id, string name)
        {

            Models.BTDevice device = new Models.BTDevice()
            {
                BT_id = bt_id,
                Name = name,
                CreatedAt = DateTime.UtcNow,
            };

            int result = await _database.SaveDeviceAsync(device);

            if(result != 1)
            {
                throw new DeviceStoreException("Saving device failed!");
            }

        }

        public async Task UpdateDeviceName(int id, string name)
        {
            Models.BTDevice device = await _database.GetDeviceAsync(id);

            if (device is null)
            {
                throw new ArgumentException("Device with given id is not saved in database!");
            }

            device.Name = name;
            int result = await _database.SaveDeviceAsync(device);

            if (result != 1)
            {
                throw new DeviceStoreException("Updating device failed!");
            }

        }

        public async Task UpdateDeviceLocation(int id, double latitude, double longitude)
        {
            Models.BTDevice device = await _database.GetDeviceAsync(id);

            if (device is null)
            {
                throw new ArgumentException("Device with given id is not saved in database!");
            }

            device.LastGPSLatitude = latitude;
            device.LastGPSLongitude = longitude;
            device.LastGPSTimestamp = DateTime.UtcNow;
            int result = await _database.SaveDeviceAsync(device);

            if (result != 1)
            {
                throw new DeviceStoreException("Updating device failed!");
            }

        }

        public async Task UpdateDeviceActive(int id, bool active)
        {
            Models.BTDevice device = await _database.GetDeviceAsync(id);

            if (device is null)
            {
                throw new ArgumentException("Device with given id is not saved in database!");
            }

            device.Active = active;
            int result = await _database.SaveDeviceAsync(device);

            if (result != 1)
            {
                throw new DeviceStoreException("Updating device failed!");
            }

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

        public async Task<List<Models.BTDevice>> GetAllDevices()
        {
            return await _database.GetAllDevicesAsync();
        }


    }

}
