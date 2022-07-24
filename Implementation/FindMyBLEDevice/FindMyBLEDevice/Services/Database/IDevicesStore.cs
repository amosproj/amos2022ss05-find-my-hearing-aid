// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using FindMyBLEDevice.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyBLEDevice.Services.Database
{
    public interface IDevicesStore
    {
        event EventHandler<List<int>> DevicesChanged;

        /// <summary>
        /// The currently selected device. 
        /// Note that this value is uninitialized when the app starts.
        /// </summary>
        BTDevice SelectedDevice { get; set; }

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
        /// <returns>The device as stored within the database</returns>
        /// <exception cref="DeviceStoreException">When operation in local database fails</exception>
        Task<BTDevice> AddDevice(BTDevice device);

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
        /// Get the device with the given guid from the devices-store
        /// </summary>
        /// <param name="guid">GUID of the requested device</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When no device with given guid is saved</exception>
        Task<BTDevice> GetDeviceByGUID(string guid);

        /// <summary>
        /// Get all saved devices from the devices store
        /// </summary>
        /// <returns>
        /// List of all saved devices saved in the local database
        /// </returns>
        Task<List<BTDevice>> GetAllDevices();

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
        Task UpdateDevice(BTDevice device);

        /// <summary>
        /// Gets the latest version of the device from the database
        /// and applies the given manipulations.
        /// </summary>
        /// <param name="device">The device to be manipulated</param>
        /// <param name="manipulation">A method that applies the manipulations to the device object.</param>
        Task AtomicGetAndUpdateDevice(int id, Action<BTDevice> manipulation);
    }
}
