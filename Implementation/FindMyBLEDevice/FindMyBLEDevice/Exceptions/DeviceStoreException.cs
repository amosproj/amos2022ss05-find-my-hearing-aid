// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo.koeberlein@fau.de>
// SPDX-FileCopyrightText: 2022 Jannik Schuetz <jannik.schuetz@fau.de>

using System;
using System.Runtime.Serialization;

namespace FindMyBLEDevice.Exceptions
{

    [Serializable]
    public class DeviceStoreException : Exception
    {

        public DeviceStoreException() {}

        public DeviceStoreException(string message) : base(message) {}

        public DeviceStoreException(string message, Exception innerException) : base(message, innerException) {}

        protected DeviceStoreException(SerializationInfo info, StreamingContext context) : base(info, context) {}

    }
}
