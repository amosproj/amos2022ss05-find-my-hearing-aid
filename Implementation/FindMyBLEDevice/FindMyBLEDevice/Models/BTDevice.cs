// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using System;
using SQLite;

namespace FindMyBLEDevice.Models
{
    /*
     * 
     *  SCHEMA FOR TABLE: BTDevice
     *  
     *      ID                  | int         | Identifier of the device, set by local database (auto-increment) | (PRIMARY KEY)
     *      BT_GUID             | string      | Technical identifier of the device used by BLUETOOTH | (UNIQUE, NOT NULL)
     *      AdvertisedName      | string      | Name of the device as advertised by itself
     *      UserLabel           | string      | Display-name of the device set by the user
     *      LastGPSLatitude     | double      | Latitude-value of the GPS-Coordinates of the last updated devices' location
     *      LastGPSLongitude    | double      | Longitude-value of the GPS-Coordinates of the last updated devices' location
     *      LastGPSTimestamp    | DateTime    | Timestamp of when the devices' location has been updated the last time (in UTC)
     *      CreatedAt           | DateTime    | Timestamp of when the device has been registered (in UTC)
     *      WithinRange         | bool        | Whether the device is currently within BLE range, updated by the UpdateService
     *      
     */
    public class BTDevice
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Unique, NotNull]
        public string BT_GUID { get; set; }

        private string _advertisedName;
        public string AdvertisedName { 
            get => _advertisedName ?? BT_GUID; 
            set { _advertisedName = value; } 
        }

        private string _userLabel;
        public string UserLabel { 
            get => _userLabel ?? AdvertisedName;
            set { _userLabel = value; }
        }

        public double LastGPSLatitude { get; set; }

        public double LastGPSLongitude { get; set; }

        public DateTime LastGPSTimestamp { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool WithinRange { get; set; }
    }
}
