// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyBLEDevice.Models
{
    public static class Constants
    {
        public const int RssiIntervalDefault = 25;
        public const int RssiIntervalMin = 0;
        public const int RssiIntervalMax = 1000;

        public const int RssiBufferDuration = 2000;
        public const int RssiBufferMaxSize = 50;

        public const int RssiTooFarThreshold = -80;

        public const int TxPowerDefault = -70;
        public const int RssiEnvironmentalDefault = 3;

        public const double MeterClosebyThreshold = 0.5;

        public const int UpdateServiceIntervalDefault = 10; //in seconds
        public const int UpdateServiceIntervalMin = 1;
        public const int UpdateServiceIntervalMax = 300;
    }
}
