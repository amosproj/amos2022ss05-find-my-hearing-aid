// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using FindMyBLEDevice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Database
{
    public interface IDevicesStore
    {

        /// <summary>
        /// Add a new device to the devices-store and local database
        /// </summary>
        /// <param name="btGuid">
        ///     The identifying ID for the Bluetooth-Technology
        /// </param>
        /// /// <param name="advertisedName">
        ///     The name of the device read from the bluetooth-signal
        /// </param>
        /// <param name="userLabel">
        ///     The name for the device given by the user
        /// </param>
        /// <returns></returns>
        /// <exception cref="DeviceStoreException">When operation in local database fails</exception>
        Task AddDevice(string btGuid, string advertisedName, string userLabel);

        /// <summary>
        /// Delete the device with given id from the devices-store and local database
        /// </summary>
        /// <param name="id">
        ///     ID of the devices to be deleted
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When no device with given id is saved</exception>
        /// <exception cref="DeviceStoreException">When operation in local database fails</exception>
        Task DeleteDevice(int id);

        /// <summary>
        /// Get the device with given id from the devices-store
        /// </summary>
        /// <param name="id">
        ///     ID of the requested device
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When no device with given id is saved</exception>
        Task<BTDevice> GetDevice(int id);

        /// <summary>
        /// Get all saved devices from the devices store
        /// </summary>
        /// <returns>
        /// List of all saved devices saved in the local database
        /// </returns>
        Task<List<BTDevice>> GetAllDevices();

        /// <summary>
        /// Update the active-flag of the device with given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When no device with given id is saved</exception>
        /// <exception cref="DeviceStoreException">When operation in local database fails</exception>
        Task UpdateDeviceActive(int id, bool active);

        /// <summary>
        /// Update the GPS-coordinates of the device with given id
        /// </summary>
        /// <param name="id">
        ///     ID of the concerned device
        /// </param>
        /// <param name="latitude">
        ///     Latitude of the devices' position
        /// </param>
        /// <param name="longitude">
        ///     Longituted of the devices' position
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When no device with given id is saved</exception>
        /// <exception cref="DeviceStoreException">When operation in local database fails</exception>
        Task UpdateDeviceLocation(int id, double latitude, double longitude);

        /// <summary>
        /// Change the name of device with given id
        /// </summary>
        /// <param name="id">
        ///     ID of the concerned device
        /// </param>
        /// <param name="userLabel">
        ///     New label for the device with given id
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When no device with given id is saved</exception>
        /// <exception cref="DeviceStoreException">When operation in local database fails</exception>
        Task UpdateDeviceUserLabel(int id, string userLabel);

    }
}