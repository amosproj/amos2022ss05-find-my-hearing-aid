// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using System;

namespace FindMyBLEDevice.Models
{
    public interface IBTDevice
    {
        int ID { get; set; }

        string BT_GUID { get; set; }

        string AdvertisedName { get; set; }

        string UserLabel { get; set; }

        DateTime CreatedAt { get; set; }

        double LastGPSLatitude { get; set; }

        double LastGPSLongitude { get; set; }

        DateTime LastGPSTimestamp { get; set; }

    }
}