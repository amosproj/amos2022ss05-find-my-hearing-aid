using System;
using System.Threading.Tasks;

namespace FindMyBLEDevice.XamarinAccess
{
    public class DeviceAccess : IDeviceAccess
    {
        public Task InvokeOnMainThreadAsync(Func<Task> func)
        {
            return Xamarin.Forms.Device.InvokeOnMainThreadAsync(func);
        }
    }
}
