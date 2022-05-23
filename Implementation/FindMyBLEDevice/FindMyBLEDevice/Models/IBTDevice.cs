// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using System;

namespace FindMyBLEDevice.Models
{
    public interface IBTDevice
    {
        bool Active { get; set; }
        string BT_id { get; set; }
        DateTime CreatedAt { get; set; }
        int Id { get; set; }
        double LastGPSLatitude { get; set; }
        double LastGPSLongitude { get; set; }
        DateTime LastGPSTimestamp { get; set; }
        string Name { get; set; }
    }
}