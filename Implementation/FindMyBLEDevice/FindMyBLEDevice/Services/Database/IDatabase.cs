// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

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
        /// BTDevice-Object of device with requested id or null if the id does not exist
        /// </returns>
        Task<BTDevice> GetDeviceAsync(int id);

        /// <summary>
        /// Retrieve a specific device from the local database by its guid asynchronously
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>
        /// BTDevice-Object of device with requested guid or null if the id does not exist
        /// </returns>
        Task<BTDevice> GetDeviceByGUIDAsync(string guid);

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