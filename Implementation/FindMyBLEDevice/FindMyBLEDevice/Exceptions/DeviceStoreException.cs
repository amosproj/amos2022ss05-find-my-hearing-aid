// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>

using System;
using System.Runtime.Serialization;

namespace FindMyBLEDevice.Exceptions
{

    [Serializable]
    public class DeviceStoreException : Exception
    {

        public DeviceStoreException()
        {
        }

        public DeviceStoreException(string message) : base(message)
        {
        }

        public DeviceStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeviceStoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
