// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@fau.de>

namespace FindMyBLEDevice.Models
{
    public static class Constants
    {
        public const int DiscoverSearchDuration = 20000;

        public const int RssiIntervalDefault = 25;
        public const int RssiIntervalMin = 0;
        public const int RssiIntervalMax = 1000;

        public const int RssiBufferDuration = 2000;
        public const int RssiBufferMaxSize = 50;

        public const int RssiTooFarThreshold = -80;

        public const int TxPowerDefault = -70;
        public const double RssiEnvironmentalDefault = 2.5;

        public const double MeterClosebyThreshold = 0.5;

        public const int UpdateServiceIntervalDefault = 5; //in seconds
        public const int UpdateServiceIntervalMin = 1;
        public const int UpdateServiceIntervalMax = 300;

        public const int UserLabelMaxLength = 15;

        public const bool DisplayNamelessDevicesDefault = false;
        public const bool DisplayWeakDevicesDefault = false;
        public const bool IncorporateGpsIntoRssiDefault = false;
    }
}
