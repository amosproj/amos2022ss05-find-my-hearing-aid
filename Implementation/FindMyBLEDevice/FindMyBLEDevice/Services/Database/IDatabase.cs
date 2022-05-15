using FindMyBLEDevice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Database
{
    public interface IDatabase
    {

        /// <summary>
        /// Delete the given device from the local database asynchronously
        /// </summary>
        /// <param name="device"></param>
        /// <returns>
        /// The number of rows deleted in the local database (should be 1 on success)
        /// </returns>
        Task<int> DeleteDeviceAsync(BTDevice device);
        
        /// <summary>
        /// Get all devices from the local database asynchronously
        /// </summary>
        /// <returns>
        /// List of all currently saved devices
        /// </returns>
        Task<List<BTDevice>> GetAllDevicesAsync();

        /// <summary>
        /// Retrieve a specific device from the local database asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// BTDevice-Object of device with requested id
        /// </returns>
        Task<BTDevice> GetDeviceAsync(int id);

        /// <summary>
        /// Save data of given device to local datavase asynchronously
        /// </summary>
        /// <param name="device"></param>
        /// <returns>
        /// The number of rows updated/inserted in the local database (should be 1 on success)
        /// </returns>
        Task<int> SaveDeviceAsync(BTDevice device);

    }
}