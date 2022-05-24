// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using System;
using SQLite;

namespace FindMyBLEDevice.Models
{

    /*
     * 
     *  SCHEMA FOR TABLE: BTDevice
     *  
     *      Id                  | int         | Identifier of the device, set by local database (auto-increment) | (PRIMARY KEY)
     *      BT_id               | string      | Technical identifier of the device used by BLUETOOTH
     *      Name                | string      | Display-name of the device set by the user
     *      CreatedAt           | DateTime    | Timestamp of when the device has been registered (in UTC)
     *      LastGPSLatitude     | double      | Latitude-value of the GPS-Coordinates of the last updated devices' location
     *      LastGPSLongitude    | double      | Longitude-value of the GPS-Coordinates of the last updated devices' location
     *      LastGPSTimestamp    | DateTime    | Timestamp of when the devices' location has been updated the last time (in UTC)
     *      Active              | boolean     | TODO: sicher, dass wir das haben wollen?
     *      
     */


    public class BTDevice : IBTDevice
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string BT_id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public double LastGPSLatitude { get; set; }

        public double LastGPSLongitude { get; set; }

        public DateTime LastGPSTimestamp { get; set; }

        public bool Active { get; set; }

    }
}
